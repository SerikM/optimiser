
# Weekdays Service API

This is a lambda api that calculates the number of weekdays between two given dates 'from' and 'to'.

The application is based on the aws serverless application model (SAM) and is written in C# and runs on .net core framework 3.1


# Build instructions
- To build and package this solution run following netcore commands from .\weekdays-api\src\weekdays directory

- To zip the packaged source files into \weekdays-api\src\weekdays\bin\Release\netcoreapp3.1\weekdays.zip run:
- dotnet lambda package 

- To run tests run following commands from .\weekdays-api\test\weekdays.Tests directory
- dotnet test


# Deployment instructions. 

  - Create an s3 bucket for the source code of your lambda by running following command from \weekdays-api\src\weekdays directory (replace the bucket name with a name of your own): 
  - aws s3 mb s3://weekdayscfs3bucket --region ap-southeast-2

  - To prepare the package for the deployment to CF and to transform the template.yml run (replace the name of the bucket with the name of your own buacket):
  - aws cloudformation package --template-file ./template.yml --output-template-file sam-template.yml --s3-bucket weekdayscfs3bucket

  - To deploy the packaged source code to aws lambda using CF run:
  - aws cloudformation deploy --template-file ./sam-template.yml --stack-name weekdays-api --capabilities CAPABILITY_IAM

  
# Testing instructions. 
 - The api can be tested using postman.
 - In order to make a simple get call with the 'from' and 'to' parametersa use following payload:
 - (The api may be run locally and when started from the VS debug menu it will be listening on http://localhost:5000/v1/default.) 

 - ex. http://localhost:5000/v1/Default?from=01/01/2020&to=05/01/2020


 
 -The following test cases can be run from postman to test the api deployed in the cloud. Replace [host_url] with the root of your api:
 
   test case #1 - perm date - Anzac Day - expected output 6</br>
   https://host_url/Prod/v1/Default?from=19/04/2020&to=28/04/2020

   test case #2 - New Years Eve - expected output 8
   https://host_url/Prod/v1/Default?from=29/11/2019&to=10/01/2020</br>

   test case #3 if we check for a period around new years eve - expected output 10</br>
   https://host_url/Prod/v1/Default?from=01/01/2020&to=16/01/2020

   test case #4 type 3 queens birthday falls on second week of June 08/06/2020 expected output 27</br>
   https://host_url/Prod/v1/Default?from=17/05/2021&to=25/06/2021

   test case #5 type 1 - 26/12/2019 Boxing day amd type 2 holiday new years eve n 01/01/2020 expected output 9</br>
   https://host_url/Prod/v1/Default?from=26/12/2019&to=10/01/2020)

   test case #6 multiple years 02/02/2019 to 29/12/2021 expected output 737</br>
   https://host_url/Prod/v1/Default?from=02/02/2019&to=29/12/2021

   test case #7 to test good friday and easter monday. Expected output 15</br>
   https://host_url/Prod/v1/Default?from=02/04/2019&to=29/04/2019

   test case #8 to test long time span expected out 2335</br>
   https://host_url/Prod/v1/Default?from=01/10/2010&to=31/12/2019
