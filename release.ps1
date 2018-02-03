
param([string]$apiKey, [string]$version, [string]$versionSuffix="");

rm dist/* -Force -Recurse

if ($versionSuffix -ne "") {
  dotnet pack /p:PackageVersion=$version source -c Debug --include-symbols --include-source --version-suffix $versionSuffix -o ../../dist
}
else {
  dotnet pack /p:PackageVersion=$version source -c Release --include-symbols --include-source -o ../../dist
  git push
  git tag $version
  git push --tags
}

nuget push ".\dist\*.nupkg" -src https://api.nuget.org/v3/index.json -ApiKey $apiKey
