Solution overview
This .NET 8 solution contains three independent web applications:
•	ApiGateway — a YARP reverse proxy that routes incoming API requests.
•	ProductService — manages products and its product database.
•	OrderService — manages orders, stores them in its database, and calls ProductService to check product details.
How a request moves through the solution
For example, a client can send POST /api/orders to the gateway:

flowchart LR
    Client["Client"] --> Gateway["ApiGateway"]
    Gateway --> Order["OrderService"]
    Order --> Product["ProductService"]
    Order --> OrderDb["Order database"]
    Product --> ProductDb["Product database"]
    
1.	The gateway receives the request.
ApiGateway matches the URL against its ReverseProxy configuration. Paths under /api/orders are forwarded to OrderService; paths under /api/products go to ProductService. The configured local addresses are https://localhost:7025/ for orders and https://localhost:7150/ for products.
2.	The relevant service handles the route.
For orders, OrdersController maps api/orders to its endpoints. It delegates the work to IServiceOrder, rather than containing the order business logic itself. ProductService uses a similar controller/service structure for products.
3.	The service applies its business logic.
ServiceOrder implements IServiceOrder. It uses AutoMapper to convert between request DTOs, database models, and response DTOs.
4.	Data access goes through a repository.
OrderRepository uses Entity Framework Core and OrderDbContext to query or modify the Orders table in the configured SQL Server database. ProductService has its own database and repository.
5.	The service returns a response.
The controllers return an HTTP result containing an ApiResponse<T> with a success flag, message, and data. For example, creating an order returns 201 Created; reads, updates, and deletes return 200 OK on success.
The active file: OrdersController.cs
The controller exposes these endpoints:
Method and path	Action
GET /api/orders	Gets all orders
GET /api/orders/{id}	Gets one order
POST /api/orders	Creates an order
PUT /api/orders/{id}	Updates an order
DELETE /api/orders/{id}	Deletes an order
Each action calls the corresponding IServiceOrder method and returns its result. The controller is the HTTP layer; ServiceOrder holds the main order logic.
What happens when an order is created
1.	The controller receives a CreateOrderRequest containing ProductId, Quantity, and TotalAmount.
2.	ServiceOrder asks ProductClient to fetch that product from ProductService.
3.	If the product is missing, or its reported stock is less than the requested quantity, the service throws an error.
4.	If validation passes, AutoMapper converts the request into an Order.
5.	The service sets the status to "Pending" and records the current UTC time.
6.	The repository saves the order, and the controller returns the created response.
ProductClient uses HttpClient with a five-second timeout. The order service also configures retries and a circuit breaker for calls to the product service.
Things to be aware of
•	Order creation checks stock but does not reserve or decrement it in ProductService.
•	Order updates change ProductId, Quantity, and TotalAmount without repeating the product or stock checks used during creation.
•	TotalAmount comes from the request; the shown order logic does not calculate it from product pricing.
•	Both services configure global exception handling, but I haven’t verified the precise HTTP status codes produced for each exception.
