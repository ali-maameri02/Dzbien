# PropeloService

**PropeloService** is a backend project built using ASP.NET Core Web API, Entity Framework, and SQL Server. This application is designed to manage real estate data, including properties, apartments for promoters, and related entities. It follows the repository pattern to ensure clean architecture and separation of concerns.

## Features

- **ASP.NET Core Web API**: Backend framework for building RESTful services.
- **Entity Framework Core**: ORM for database interaction.
- **Repository Pattern**: Abstracts data access to maintainable and extendable architecture.
- **SQL Server**: Primary relational database for persisting data.
- **Domain Structure**: Manages entities such as Promoters, Properties, Apartments, Pictures, Documents, and more.

## Architecture Overview

This project follows the repository pattern to separate the data access layer from business logic. The class diagram below illustrates the key domain entities and their relationships.

### Key Entities

- **User**: Stores user login credentials (Username, Password).
- **Promoter**: Represents a real estate promoter with details such as name, contact information, and profile picture.
- **Property**: Stores property information such as address, geographical location, construction dates, and the number of apartments.
- **Apartment**: Defines individual apartments under properties, including rooms, surface area, floor, and descriptions.
- **Areas**: Represents apartment areas such as surface size and room details.
- **Document**: Manages documents related to apartments or properties, including file references.
- **Picture**: Stores pictures associated with Promoters, Properties, and Apartments.
- **Settings**: Holds global settings for the system, including the application name and logo.

### Entity Relationships

- **Promoter and User**: One-to-one relationship.
- **Property and Promoter**: One-to-one relationship.
- **Property and Apartment**: One-to-many relationship.
- **Apartment and Picture**: One-to-many relationship.
- **Apartment and Document**: One-to-many relationship.

## Project Structure

- **Controllers**: Handle HTTP requests and interact with services or repositories.
- **Services**: Contain business logic and coordinate different operations.
- **Repositories**: Data access layer using Entity Framework to perform database interactions.
- **Models**: Entity models representing the database tables.
- **DTOs**: Data Transfer Objects encapsulating data for transfer between the server and client.

## Database Schema

Here are the main tables in the SQL Server database:

- `Users`
- `Promoters`
- `Properties`
- `Apartments`
- `Areas`
- `Pictures`
- `Documents`
- `Settings`

## Technologies

- **ASP.NET Core Web API**: Backend framework for building services.
- **Entity Framework Core**: ORM to simplify database operations.
- **SQL Server**: Database for persisting application data.
- **Repository Pattern**: Ensures separation of concerns and scalable architecture.
- **AutoMapper**: Maps data between models and DTOs.
- **Dependency Injection**: For managing services and repositories.

## Class Diagram

The class diagram below illustrates the relationships between entities in the project.

![Propelo Class Diagram](https://github.com/user-attachments/assets/eebc42aa-9ac6-49cf-9df9-c854a14cd9b4)
