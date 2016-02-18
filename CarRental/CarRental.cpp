#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <list>
#include <string>

using namespace std;

class Brand {
public:
	static string BMW() {
		return "BMW";
	}
	static string AUDI() {
		return "AUDI";
	}
	static string FORD() {
		return "FORD";
	}
	static string TOYOTA() {
		return "TOYOTA";
	}
};

class Car {
protected:
	string licencePlate;
	int numSeats;
public:
	Car() {}
	virtual string GetLicencePlate() {
		return licencePlate;
	}
	virtual int GetSeatCount() {
		return numSeats;
	}
	virtual string GetCarType() {
		return "";
	}
	virtual string GetCarBrand() {
		return "";
	}
};

class SUV : public Car {
private:
	bool isOptionEnabled = true;
	string brand;
	string type;
	int optionalSeats = 3;	//assuming 3 people can sit on the optional third row seat..
public:
	SUV(string lic, string br, int seats) {
		licencePlate = lic;
		brand = br;
		numSeats = seats;
		type = "SUV";
	}
	void DisableOption() {
		isOptionEnabled = false;
	}
	void EnableOption() {
		isOptionEnabled = true;
	}
	bool IsOptionEnabled() {
		return isOptionEnabled;
	}
	int GetSeatCount() {
		if (isOptionEnabled) {
			return numSeats + optionalSeats;
		}
		else return numSeats;
	}
	string GetCarBrand() {
		return brand;
	}
	string GetCarType() {
		return type;
	}
};

class Sedan : public Car {
private:
	bool optionalSportsPkg = true;
	string brand;
	string type;
public:
	Sedan(string lic, string br, int seats) {
		licencePlate = lic;
		brand = br;
		numSeats = seats;
		type = "Sedan";
	}
	void DisableOption() {
		optionalSportsPkg = false;
	}
	void EnableOption() {
		optionalSportsPkg = true;
	}
	bool IsOptionEnabled() {
		return optionalSportsPkg;
	}
	int GetSeatCount() {
		return numSeats;
	}
	string GetCarType() {
		return type;
	}
	string GetCarBrand() {
		return brand;
	}
};

class CarRental {
private:
	static CarRental* instance;
	list<Car*> Inverntory_avail;
	list<Car*> Inverntory_rented;
	CarRental() {
	}	//private constructor
public:
	static CarRental* GetInstance() {
		if (instance == NULL) {
			instance = new CarRental();
		}
		return instance;
	}
	int availabeCount() {
		return Inverntory_avail.size();
	}
	int rentedCount() {
		return Inverntory_rented.size();
	}
	int totalFleetCount() {
		return (availabeCount() + rentedCount());
	}
	void AddCarToInventory(Car* c) {
		Inverntory_avail.push_back(c);
	}
	Car * RentCar() {
		Car * ret = NULL;
		if (!Inverntory_avail.empty()) {
			ret = Inverntory_avail.front();
			Inverntory_rented.push_back(ret);
			Inverntory_avail.pop_front();
			return ret;
		}
		else {
			return NULL;
		}
	}
	void ReturnCar(Car * c) {
		if (c == NULL) {
			if (Inverntory_rented.size()) {
				c = Inverntory_rented.front();
				Inverntory_avail.push_back(c);
				Inverntory_rented.pop_front();
				cout << "Returned Car Details: " << endl;
				cout << "Licence#: " << c->GetLicencePlate() << "   Maximum Seating Capacity: " << c->GetSeatCount() << endl;
				cout << "Type:     " << c->GetCarType() << "      Brand: " << c->GetCarBrand() << endl;
			}
			else {
				cout << "We do not have any vehicles rented out to be returned, add the vehicle via inventory." << endl;
			}
		}
		else {
			Inverntory_avail.push_back(c);
			Inverntory_rented.remove(c);
			cout << "Returned Car Details: " << endl;
			cout << "Licence#: " << c->GetLicencePlate() << "   Maximum Seating Capacity: " << c->GetSeatCount() << endl;
			cout << "Type:     " << c->GetCarType() << "      Brand: " << c->GetCarBrand() << endl;
		}
	}
};
CarRental* CarRental::instance = NULL;


int main()
{
	CarRental* CR = CarRental::GetInstance();
	Car * c;

	c = new SUV("SUV001", Brand::AUDI(), 4);
	CR->AddCarToInventory(c);
	c = new SUV("SUV002", Brand::FORD(), 4);
	CR->AddCarToInventory(c);
	c = new SUV("SUV003", Brand::TOYOTA(), 4);
	CR->AddCarToInventory(c);
	c = new SUV("SUV004", Brand::FORD(), 4);
	CR->AddCarToInventory(c);
	c = new SUV("SUV005", Brand::BMW(), 4);
	CR->AddCarToInventory(c);

	c = new Sedan("Sed001", Brand::BMW(), 4);
	CR->AddCarToInventory(c);
	c = new Sedan("Sed002", Brand::AUDI(), 2);
	CR->AddCarToInventory(c);
	c = new Sedan("Sed003", Brand::FORD(), 4);
	CR->AddCarToInventory(c);
	c = new Sedan("Sed004", Brand::TOYOTA(), 2);
	CR->AddCarToInventory(c);
	c = new Sedan("Sed005", Brand::AUDI(), 4);
	CR->AddCarToInventory(c);

	int choice;
	while (true) {

		cout << endl << "*****Enter Menu Selection*****" << endl;
		cout << "Total     Fleet   Count:   " << CR->totalFleetCount() << endl;
		cout << "Available Car     Count:   " << CR->availabeCount() << endl;
		cout << "Rented    Car     Count:   " << CR->rentedCount() << endl;
		cout << "1: Rent a Car     2: Return a Car" << endl;
		cout << "3: Close Application and Leave" << endl;
		cout << "******************************" << endl << endl;

		Car* c;
		SUV* suv;
		Sedan* sedan;
		cin >> choice;
		switch (choice)
		{
		case 1:
			c = CR->RentCar();
			if (c != NULL) {
				cout << "Licence#: " << c->GetLicencePlate() << "   Maximum Seating Capacity: " << c->GetSeatCount() << endl;
				cout << "Type:     " << c->GetCarType() << "      Brand: " << c->GetCarBrand() << endl;
			}
			else {
				cout << "We ran out of cars at this moment." << endl;
			}
			break;
		case 2:
			CR->ReturnCar(NULL);
			break;
		case 3:
			exit(1);
		default:
			break;
		}
	}
	getchar();
	return 0;
}