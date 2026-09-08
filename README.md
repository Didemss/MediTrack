# MediTrack

MediTrack is a RESTful healthcare management API developed with ASP.NET Core and PostgreSQL.  
It provides a backend system for managing patients, doctors, appointments, medical records, and prescriptions with JWT-based authentication.

## Features

- User registration and login
- JWT authentication and protected endpoints
- Patient management
- Doctor management
- Appointment management
- Medical record management
- Prescription management
- PostgreSQL database integration
- Entity Framework Core
- Layered architecture

## Technologies

- C#
- ASP.NET Core Web API
- .NET 9
- Entity Framework Core
- PostgreSQL
- Npgsql
- JWT Authentication
- Swagger / OpenAPI

## Project Structure

```text
MediTrack
├── src
│   ├── MediTrack.API
│   ├── MediTrack.Application
│   ├── MediTrack.Domain
│   └── MediTrack.Infrastructure
├── tests
├── MediTrack.sln
└── README.md
```

## Main API Resources

- `/api/auth`
- `/api/patients`
- `/api/doctors`
- `/api/appointments`
- `/api/medicalrecords`
- `/api/prescriptions`

## Authentication

MediTrack uses JSON Web Tokens (JWT) for authentication. After successful login, authenticated users can access protected API endpoints using a Bearer token.

## Database

The project uses PostgreSQL with Entity Framework Core for data persistence and migrations.

## Purpose

This project was developed to practice and demonstrate backend development concepts including REST API design, layered architecture, authentication, relational database management, and Entity Framework Core.