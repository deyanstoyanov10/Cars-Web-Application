# Asp.Net-Core-Cars-WebApplication

Project example Asp.Net Core Mvc implementation cars web application.
Add new car, search car, real time message system, machine learning calculator for prediction car price.
For machine learning algorithm i am using LbfgsPoissonRegression.

### Installing

You can deploy this site in azure with a few steps.

* Create new resource group in azure (web + sql);
* Run this project in visual studio
* Go to appsettings.json and change DefaultConnection string with yours. Also change default admin account and password with yours.
* Go to StartUp.cs and scroll down to fonction "CreateUserRoles" and add or remove roles if it needs.
* Also scroll down and add, remove or change current brand and model type of cars.
* Now you are ready to start this project for first time, so go to azure and download publish profile.
* Go to visual studio and right click on CarsWebApp and click publish
* Clcik on Import Profile and upload that profile from azure.
* Click on publish and enjoy.

### Features

* Asp.Net Core Mvc
* EF / Entity Framework
* Code First
* C#
* Beautiful Bootstrap
* JQuery + Ajax
* Machine learning

### Functional Features

* Edit Profile
* Change Password
* Real time message system
* Add new car
* Search cars
* Predict Car Price

### Development Tools & Environment

* Visual Studio 2017 (Community Edition). [(https://visualstudio.microsoft.com/)](https://visualstudio.microsoft.com/)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Features in Development

* Adding admin panel
* Adding Ads with payment
* Fix message system perfomance
* Elastic search
