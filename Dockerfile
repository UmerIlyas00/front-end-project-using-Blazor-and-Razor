# Get app platform framework sdk in working folder 
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app

# Client app
COPY ./DotnetEFRazorCompSSR.App ./DotnetEFRazorCompSSR.App
RUN dotnet build ./DotnetEFRazorCompSSR.App/DotnetEFRazorCompSSR.App.csproj 

# Server app
COPY ./DotnetEFRazorCompSSR.Server ./DotnetEFRazorCompSSR.Server
RUN dotnet build ./DotnetEFRazorCompSSR.Server/DotnetEFRazorCompSSR.Server.csproj -c Release 

# Unit tests for apps
COPY ./DotnetEFRazorCompSSR.Tests ./DotnetEFRazorCompSSR.Tests
RUN dotnet build ./DotnetEFRazorCompSSR.Tests/DotnetEFRazorCompSSR.Tests.csproj 
RUN dotnet test ./DotnetEFRazorCompSSR.Tests/DotnetEFRazorCompSSR.Tests.csproj 

# Generate final app output
RUN dotnet publish ./DotnetEFRazorCompSSR.Server/DotnetEFRazorCompSSR.Server.csproj -c Release -o out --no-restore

# Publish the final app with runtime
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=builder /app/DotnetEFRazorCompSSR.Server/out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet DotnetEFRazorCompSSR.Server.dll