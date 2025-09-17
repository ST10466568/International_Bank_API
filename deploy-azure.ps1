# Azure App Service Deployment Script for Hopewell Clinic API
# This script deploys the published application to Azure App Service

Write-Host "Starting Azure App Service Deployment..." -ForegroundColor Green

# Check if Azure CLI is installed
try {
    $azVersion = az --version
    Write-Host "Azure CLI is installed" -ForegroundColor Green
} catch {
    Write-Host "Azure CLI is not installed. Please install it first." -ForegroundColor Red
    Write-Host "Download from: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli" -ForegroundColor Yellow
    exit 1
}

# Check if user is logged in to Azure
try {
    $account = az account show --query "name" -o tsv
    if ($account) {
        Write-Host "Logged in to Azure as: $account" -ForegroundColor Green
    } else {
        Write-Host "Not logged in to Azure. Please run 'az login' first." -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "Not logged in to Azure. Please run 'az login' first." -ForegroundColor Red
    exit 1
}

# Configuration variables
$resourceGroupName = "hopewell-clinic-rg"
$appServiceName = "hopewell-clinic-api"
$location = "South Africa North"
$publishPath = "./publish"

# Check if resource group exists
Write-Host "Checking resource group..." -ForegroundColor Yellow
$rgExists = az group exists --name $resourceGroupName --output tsv

if ($rgExists -eq "false") {
    Write-Host "Creating resource group: $resourceGroupName" -ForegroundColor Yellow
    az group create --name $resourceGroupName --location "$location"
} else {
    Write-Host "Resource group exists: $resourceGroupName" -ForegroundColor Green
}

# Check if App Service Plan exists
Write-Host "Checking App Service Plan..." -ForegroundColor Yellow
$planExists = az appservice plan list --resource-group $resourceGroupName --query "[?name=='hopewell-clinic-plan']" --output tsv

if (-not $planExists) {
    Write-Host "Creating App Service Plan: hopewell-clinic-plan" -ForegroundColor Yellow
    az appservice plan create --resource-group $resourceGroupName --name "hopewell-clinic-plan" --sku "F1" --is-linux
} else {
    Write-Host "App Service Plan exists: hopewell-clinic-plan" -ForegroundColor Green
}

# Check if App Service exists
Write-Host "Checking App Service..." -ForegroundColor Yellow
$appExists = az webapp list --resource-group $resourceGroupName --query "[?name=='$appServiceName']" --output tsv

if (-not $appExists) {
    Write-Host "Creating App Service: $appServiceName" -ForegroundColor Yellow
    az webapp create --resource-group $resourceGroupName --plan "hopewell-clinic-plan" --name $appServiceName --runtime "DOTNET|8.0"
} else {
    Write-Host "App Service exists: $appServiceName" -ForegroundColor Green
}

# Create a zip file of the publish directory
Write-Host "Creating deployment package..." -ForegroundColor Yellow
Compress-Archive -Path "$publishPath/*" -DestinationPath "./publish.zip" -Force

# Deploy the zip file
Write-Host "Deploying to Azure App Service..." -ForegroundColor Yellow
az webapp deployment source config-zip --resource-group $resourceGroupName --name $appServiceName --src "./publish.zip"

# Get the app URL
$appUrl = az webapp show --resource-group $resourceGroupName --name $appServiceName --query "defaultHostName" -o tsv
$fullUrl = "https://$appUrl"

Write-Host "Deployment completed!" -ForegroundColor Green
Write-Host "Application URL: $fullUrl" -ForegroundColor Cyan
Write-Host "API Documentation: $fullUrl/swagger" -ForegroundColor Cyan

# Test the deployment
Write-Host "Testing deployment..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$fullUrl/health" -Method GET -TimeoutSec 30
    Write-Host "Health check passed: $response" -ForegroundColor Green
} catch {
    Write-Host "Health check failed, but deployment may still be successful" -ForegroundColor Yellow
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Clean up
Remove-Item "./publish.zip" -Force -ErrorAction SilentlyContinue

Write-Host "Deployment process completed!" -ForegroundColor Green
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "   1. Update your frontend to use the new API URL: $fullUrl" -ForegroundColor White
Write-Host "   2. Test the booking endpoints: $fullUrl/api/Booking/doctors-on-duty?date=2025-09-19" -ForegroundColor White
Write-Host "   3. Monitor the application in Azure Portal" -ForegroundColor White