# optimiser
commercial optimiser - its purpose is to calculate optimal sequence of commercials based on demographics and time of day


# Optimiser Service API

This is a lambda api that calculates 

The application is based on the aws serverless application model (SAM) and is written in C# and runs on .net core framework 3.1


# Build instructions
- To build and package this solution run following netcore commands from .\optimiser\api\src\optimiser directory 

- To zip the packaged source files into \Optimiser-api\src\Optimiser\bin\Release\netcoreapp3.1\Optimiser.zip run:
- dotnet lambda package 

- To run tests run following commands from .\Optimiser-api\test\Optimiser.Tests directory
- dotnet test


# Deployment instructions. 

  - Create an s3 bucket for the source code of your lambda by running following command from \optimiser\api\src\Optimiser directory: 
  - aws s3 mb s3://optimiser-api --region ap-southeast-2

  - To prepare the package for the deployment to CF and to transform the template.yml run:
  - aws cloudformation package --template-file ./template.yml --output-template-file sam-template.yml --s3-bucket optimiser-api

  - To deploy the packaged source code to aws lambda using CF run:
  - aws cloudformation deploy --template-file ./sam-template.yml --stack-name optimiser-api --capabilities CAPABILITY_IAM

  
# Testing instructions.
