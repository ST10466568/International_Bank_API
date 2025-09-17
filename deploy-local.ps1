# Local Deployment Script for Hopewell Clinic API
# This script runs the published application locally

Write-Host "🚀 Starting Local Deployment..." -ForegroundColor Green

# Check if publish directory exists
if (-not (Test-Path "./publish")) {
    Write-Host "❌ Publish directory not found. Please run 'dotnet publish' first." -ForegroundColor Red
    exit 1
}

# Check if the main DLL exists
if (-not (Test-Path "./publish/HopewellClinicApi.dll")) {
    Write-Host "❌ Published application not found. Please run 'dotnet publish' first." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Published application found" -ForegroundColor Green

# Stop any existing instances
Write-Host "🛑 Stopping existing instances..." -ForegroundColor Yellow
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Where-Object { $_.MainWindowTitle -like "*HopewellClinicApi*" } | Stop-Process -Force -ErrorAction SilentlyContinue

# Start the application
Write-Host "🚀 Starting Hopewell Clinic API..." -ForegroundColor Yellow
Write-Host "📍 Application will be available at: http://localhost:5002" -ForegroundColor Cyan
Write-Host "📋 API Documentation: http://localhost:5002/swagger" -ForegroundColor Cyan
Write-Host "🔍 Health Check: http://localhost:5002/health" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host ""

# Run the application
Set-Location "./publish"
dotnet HopewellClinicApi.dll --urls="http://localhost:5002"

