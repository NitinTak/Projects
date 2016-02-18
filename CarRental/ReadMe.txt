Design:
A car rental company wants to keep track of its cars. Each vehicle has a license plate and a brand. (eg. BWM). Currently the company has SUVs and Sedans. SUV-s have an optional third row seat, sedans have an optional sport package. Each car can be queried to inquire the number of passengers it can carry.

The solution was developed using Visual Studio 2015 on C++
Design is as follows:

class Brand:
Class to just hold name strings for different brands available, some have been added for illustration. 

class Car:
Base class Car, to have basic attributes like "Licence plate number" and "Number of seats" that any car might have.
GetLicencePlate() function is defined as virtual to get Licence plate Number for any car.
GetSeatCount() function is defined as virtual and will be overrided by derived class to add any seats in case increased number of seats is an option provided in particular car type.
GetCarType() function defined as virtual is just a placeholder to override in the derived class to return respective car type.
GetCarBrand() function defined as virtual is just a placeholder to override in the derived class to return respective car brand.

class SUV extends class CAR:
ASSUMPTIONS: 
1. By default third row option is enabled which can be disabled.
2. The option of a third row increases number of seats by 3.
Contains information about any additional features/options available for that particular car type and an identifier if the option is enabled.
Constructor is provided to initialize the SUV while creation. 
Functions have been provided to update/modify the options enabled.

class Sedan extends class CAR:
ASSUMPTIONS: 
1. By default sports package option is enabled which can be disabled.
2. The option of sports package does not affect number of seats.
Contains information about any additional features/options available for that particular car type and an identifier if the option is enabled.
Constructor is provided to initialize the Sedan while creation. 
Functions have been provided to update/modify the options enabled.

class CarRental:
Designed as a singleton, holds the data for the application in a single instance. Data is segregated as available as rented and available cars.
Basic functions are provided to perform basic operations like adding a car to fleet and renting and returning a car for Car Rental which can be further extended based on requirements.
Function RentCar() gets the first car available irrespective of user preferances but can be changed as requirements.
Function returnRentedCar assumes that the cars are returned in a first-out first-in pattern and this behaviour can be overridden by passing a specific the car pointer as a parameter to the function for SUV and Sedan respectively.

int main():
Main function adds some Cars of different brands and types for illustration purposes and does menu driven operations based on user choice. Any operation available to SUV/Sedan class and not available to Car class can be done by typecasting the Car pointer into SUV/Sedan by checking the type but this is assumed to be out of scope for this illustration.
