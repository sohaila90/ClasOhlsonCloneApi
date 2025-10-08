# Clas Ohlson Online Store API

This is the backend for my school project — a simple clone of the Clas Ohlson online store.
It provides API endpoints for products, users, and the shopping cart.

## Technology
- **ASP.NET Core Web API (.NET 9)**
- **C# 13**
- **Entity Framework Core (MySQL)**
- **Scalar / Swagger** for API documentation

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

# Get all products
```bash
curl http://localhost:5050/Product
```

# Get a product by ID
```bash
curl http://localhost:5050/Product/1
```

## Users

# Get all users

```bash
curl http://localhost:5050/User
```

# Get a user by ID

```bash
curl http://localhost:5050/User/1
```

## Cart

# View cart

```bash
curl http://localhost:5050/Cart
```

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