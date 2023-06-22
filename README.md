- [setur-assessment](#setur-assessment)
  - [Making necessary configuration in practice](#making-necessary-configuration-in-practice)
  - [Standing up services in sequence](#standing-up-services-in-sequence)
  - [Access to Services](#access-to-services)
# setur-assessment
This project was designed and implemented for use in the recruitment process. The aim of the project is a small phonebook application. It contains the APIs necessary for this purpose.

## Making necessary configuration in practice
In the appsettings.json file (`~/PhoneBook/src/Services/PhonebookService/PhonebookService/PhonebookService.Api/appsettings.json`), please add your ConnectionString for the database connection.

Then we have to do the same for our other service, the ReportingService(`~/PhoneBook/src/Services/PhonebookService/PhonebookService/PhonebookService.Api/appsettings.json`).

## Standing up services in sequence
You can start our services in the following order.

1. ApiGateways
2. Phonebook Services
3. Reporting Services

The above projects are started by default from ports **localhost:5001**, **localhost:5002**, **localhost:5003** respectively.

## Access to Services
Services can be accessed through their own ports, but since there is an apigateway project in the project, services can be accessed in the following ways.

1. Access services through their own ports.
   - Phonebook Service => http://localhost:5002/api/Phonebook/systemCheck
   - Reporting Service => http://localhost:5003/api/Reporting/systemCheck

2. Access via ApiGateway.
   - Phonebook Service => http://localhost:5001/Phonebook/systemCheck
   - Reporting Service => http://localhost:5001/Reporting/systemCheck
