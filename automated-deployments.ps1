# Load .env file
$envFile = ".env"
if (Test-Path $envFile) {
    Get-Content $envFile | ForEach-Object {
        if ($_ -match "^\s*([^#].*?)\s*=\s*(.*)\s*$") {
            $name, $value = $matches[1], $matches[2]
            [System.Environment]::SetEnvironmentVariable($name, $value)
        }
    }
} else {
    Write-Host ".env file not found in current directory"
    exit 1
}

# Variables from .env
$dockerUser   = $env:DOCKER_USER
$dockerPass   = $env:DOCKER_PASS
$imageName    = $env:IMAGE_NAME
$imageTag     = $env:IMAGE_TAG
$renderApiKey = $env:RENDER_API_KEY
$serviceId    = $env:SERVICE_ID

# 1. Build Docker image
docker build -t "${imageName}:${imageTag}" .

if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker build failed"
    exit 1
}

# 2. Docker login with PAT
docker login -u $dockerUser -p $dockerPass

if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker login failed"
    exit 1
}

# 3. Push Docker image
docker push "${imageName}:${imageTag}"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker push failed"
    exit 1
}

Write-Host "Docker image pushed successfully"

# 4. Trigger Render deploy
$headers = @{
  "Authorization" = "Bearer $renderApiKey"
  "Content-Type"  = "application/json"
}

$response = Invoke-RestMethod -Uri "https://api.render.com/v1/services/$serviceId/deploys" `
                              -Headers $headers -Method Post -Body '{}'

Write-Host "Render deployment triggered: $($response.id)"
