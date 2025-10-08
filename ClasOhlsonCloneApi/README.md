# Clas Ohlson Online Store API

This is the backend for my school project — a simple clone of the Clas Ohlson online store.
It provides API endpoints for products, users, and the shopping cart.

---

## Technology
- **ASP.NET Core Web API (.NET 9)**
- **C# 13**
- **Entity Framework Core (MySQL)**
- **Scalar** for API documentation

## Run Locally

### Clone the project
```bash
git clone https://github.com/yourusername/ClasOhlsonCloneApi.git
cd ClasOhlsonCloneApi
dotnet restore
dotnet run
```

## Install dependencies
```bash
dotnet restore
```

## Run the project
```bash
dotnet run
```

Then open your browser at:
```bash
http://localhost:5050
```

## API Endpoints

Products
```bash
GET  /Products
GET  /Products/{id}
```

## Users

```bash
GET  /Users
GET  /Users/{id}
POST /Users/register
POST /Users/login
```

## Cart

```bash
GET    /Cart
POST   /Cart/add
DELETE /Cart/remove
```

## API Docs
Once running, you can test all endpoints in your browser via:
- Scalar UI: http://localhost:5050/scalar/v1
- Swagger UI: http://localhost:5050/swagger/index.html

# Add item to cart

```bash
curl http://localhost:5050/Cart/add \
  --request POST \
  --header 'Content-Type: application/json' \
  --data '{
    "productId": 1,
    "quantity": 1,
    "userId": 1
  }'
```