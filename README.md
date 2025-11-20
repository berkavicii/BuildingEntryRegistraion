# Building Entry Registration
## Project Structure
BuildingEntryRegistration.Api/   → .NET Web API
BuildingEntryRegistration.Web/   → Angular frontend
Test/                            → Backend unit tests

## Run the Backend (API)

1. Open terminal  
2. Navigate to the API folder:

   ```bash
   cd BuildingEntryRegistration.Api
   dotnet restore
   dotnet run

API will start at 	
  •	http://localhost:5068
	•	Swagger: http://localhost:5068/swagger

## Run the Frontend (Angular)
Open a new terminal
Navigate to Web folder:
cd BuildingEntryRegistration.Web

Install dependencies: npm install
start the Angular Project: ng serve --open

Angular runs at:
	•	http://localhost:4200

If having a CORS problem because of using another url
-> In Api project Program.cs.AddCors -> policy.WithOrigins = (should be new port)



## Running Backend Tests

cd Test
dotnet Test



