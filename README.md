## Optimiser
Commercial Optimiser - is an app that calculates the optimal combination and the optimal rating score for a set of TV commercials based on several criteria. 

Default settings:<br/>
- 3 demographics (Women 25-30, Men 18-35 &amp; Total People 18-40)
- 9 commercials (each commercial has a demographic it will target)
- 3 commercial breaks with ratings per demographic
Restrictions: <br/>
- Finance type commercials cannot go into Break 2
- Commercials of the same type cannot be next to each other within a break

The application supports all types of mobile devices and works well on desktops. The default commercials data is stored in the database and is loaded into the application at startup. The user is given the ability to enter new rating for each commercial for a corresponding brake. 







# Optimiser Service API

This is a lambda api that calculates 

The application is based on the aws serverless application model (SAM) and is written in C# and runs on .net core framework 3.1


## Build instructions

> to build and package the lambda project locally navigate to .\optimiser\api\src\optimiser directory and run
```shell
dotnet lambda package 
```
> this command will zip the packag into \Optimiser-api\src\Optimiser\bin\Release\netcoreapp3.1\Optimiser.zip

> to run tests navigate to .\Optimiser-api\test\Optimiser.Tests directory and run
```shell
 dotnet test
```

## Table of Contents

> Create an s3 bucket for the source code of your lambda by running following command from \optimiser\api\src\Optimiser directory: 
```shell
aws s3 mb s3://optimiser-api --region ap-southeast-2
```
> To prepare the package for the deployment to CloudFormation and to transform the template.yml run:
```shell
aws cloudformation package --template-file ./template.yml --output-template-file sam-template.yml --s3-bucket optimiser-api
```

  - To deploy the packaged source code to aws lambda using CF run:
  - aws cloudformation deploy --template-file ./sam-template.yml --stack-name optimiser-api --capabilities CAPABILITY_IAM

  
# Testing instructions.
 Currently the app is deployed here <a href="http://optimiser-app.s3-website-ap-southeast-2.amazonaws.com" target="_blank">here</a>
