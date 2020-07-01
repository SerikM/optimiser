# Optimiser

Commercial Optimiser - is an app that determinnes the optimal allocation and calculates the achieved rating score for a set of TV commercials based on several criteria. 

Default settings:<br/>
- 3 demographics (Women 25-30, Men 18-35 &amp; Total People 18-40)
- 9 commercials (each commercial has a demographic it will target)
- 3 commercial breaks with ratings per demographic

Restrictions: <br/>
- Finance type commercials cannot go into Break 2
- Commercials of the same type cannot be next to each other within a break

The application supports all types of mobile devices and works well on desktops. The default commercials data is stored in the database and is loaded into the application at startup. The user is given the ability to enter new rating for each commercial for a corresponding brake. 


# Architecture

- This application follows the modern serverless design and the best practices of building and hosting cloud based serverless public facing applications. 
- The application consists of two main parts - the `Reactjs UI app` and the `.NET Core Lambda api`.
- This dessign achieves great economy as it makes unnecessary running a full-scale web server and this is also in line with all the principles of the microservices architecture.

## UI app
- The UI app is built using ReactJs and Bootstrap 4. It is compiled and then copied over as a package of static files into AWS S3 bucket. The bucket is set up as a static file server. In theory the UI part can be hosted on any sevrer capable of serving static or dynamic applications. It does not require IIS, Kestrel, NodeJs or any other specific server. Once loaded into the browser the app will start making `REST` calls to the the backend service which is also hosted in AWS as a serverless Lambda. In theory the backend service can be swapped with another service capable of serving content over RESTful connections. 
- The UI app supports mobile devices and desktops equally well.

## Lambda api
- The api is based on the .NET Core 3.1 AWS Lambda template. The logic is implemented using C#.
- The api is run as a serverless application.
- The api uses the serverless database technology `DynamoDb` as it's main storage provider
- All the default applicartion data like ratrings, commercial ids etc are retrieved from DynamoDb at startup.
- The api uses two DynamoDb data tables for its operation. The tables' get provisioned during the initial deployment of the api (procedure is decribed below). 


## UI app build instructions
- The app can be built and run locally
> to start the app navigate to .\optimizer\app directory and run
```shell
npm run start
```
> to build the app for deployment run 
```shell
npm run start
```
> now you can copy the files from the build folder to the server of your choice


## API build instructions
> to build and package the lambda project locally navigate to .\optimiser\api\src\optimiser directory and run
```shell
dotnet lambda package 
```
> this command will zip the packag into \Optimiser-api\src\Optimiser\bin\Release\netcoreapp3.1\Optimiser.zip

> to run tests navigate to .\Optimiser-api\test\Optimiser.Tests directory and run
```shell
 dotnet test
```

## API deployment instructions
> In order to use commands given below you'll need to replace [my-bucket-name] with the name of your own s3 bucket

> Create an s3 bucket for the source code of your lambda by running following command from \optimiser\api\src\Optimiser directory: 
```shell
aws s3 mb s3://[my-bucket-name] --region ap-southeast-2
```

> To prepare the package for the deployment to CloudFormation and to transform the template.yml run:
```shell
aws cloudformation package --template-file ./template.yml --output-template-file sam-template.yml --s3-bucket [my-bucket-name]
```

> To deploy the packaged source code to aws lambda using CF run:
```shell
aws cloudformation deploy --template-file ./sam-template.yml --stack-name [my-bucket-name] --capabilities CAPABILITY_IAM
```

## DynamoDb data seed instructions
> initially the data tables are provisioned empty. In order to populate them with data the PUT api can be called, provided the api where lambda has been deployed is known. This will populate the tables with default data defined in code.
```shell
PUT https://[lambda-hostname]/Prod/v1/default
```
  
  
# Testing instructions.
 Currently the app is deployed here <a href="http://optimiser-app.s3-website-ap-southeast-2.amazonaws.com" target="_blank">here</a>.
