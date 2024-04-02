# Rainfall API

This project is a .NET Core API that interacts with the Environment Agency Rainfall API to retrieve rainfall data from measurement stations around the UK. The API provides endpoints to fetch rainfall readings by station ID, following the specifications outlined in the provided OpenAPI documentation.

[Coding Test API Documentation](https://drive.google.com/file/d/1QP4KO2pg_IItEex6HSGF8pTKzzPEuKH5/view)

## Dependencies
- .NET Core 6
- Swashbuckle.AspNetCore (for Swagger documentation)
- Newtonsoft.Json (for JSON serialization)
- HttpClient (for making HTTP requests)
- AutoMapper (for mapping objects)
- NUnit (for unit testing)
- Moq (for mocking dependencies in unit tests)

## How to Run the Project
1. Clone the repository.
2. Navigate to the RainfallApi directory.
3. Open the solution file (RainfallApi.sln) in Visual Studio 2022 or Visual Studio Code.
4. Restore the NuGet packages for the project.
5. Build the project to ensure all dependencies are resolved.
6. Run the project by pressing F5 or using the appropriate command in the terminal.
7. Access the API documentation using Swagger UI at [https://localhost:<port>/swagger/index.html](https://localhost:<port>/swagger/index.html).

## Note
Refer to the Swagger documentation generated for the API to understand the available endpoints and their functionalities.
This README.md provides an overview of the project, its dependencies, and instructions on how to run the .NET Core API that interacts with the Environment Agency Rainfall API.
