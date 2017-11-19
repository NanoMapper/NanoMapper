
param([string]$versionSuffix="",[string]$apiKey="");


rm dist/*

if ($versionSuffix -ne "") {
  dotnet pack source -c Debug --include-symbols --include-source --version-suffix $versionSuffix -o ../../dist
}
else {
  dotnet pack source -c Release --include-symbols --include-source -o ../../dist
}

nuget push ".\dist\*.nupkg" -src https://api.nuget.org/v3/index.json -ApiKey $apiKey
