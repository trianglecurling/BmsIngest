# BMS Ingest
Build to retrieve data from the BMS system running the chiller,
and insert into an InfluxDB for display/processing by other tools.

This is built as an ASP.Net Core application, with a scheduled job
running every minute to retrieve data, transform it slightly, and
load into InfluxDB.

## Running ##
This was built to run in a Docker container. Future updates will add
GitHub actions to build the container and push it to Docker Hub.

## Configuration ##
Configuration is done via appsettings.json, which should *NOT* contain
any secrets or sensitive info. Secrets and sensitive info should be passed
in with environment variables, or other methods supported by ASP.Net Core.

## Web Interface ##
The application has a web interface, which is currently only for testing purposes.
There is a single endpoint, at `/api/test/GetInformation`. This takes an HTTP POST
request, with no parameters or authentication. It runs a query on the BMS system,
and returns the data in the transformed format, serialized to JSON. This endpoint
not insert the data into InfluxDB at all

## Logging ##
Some minimal logging is done to the console. This will be expanded "soon" or "eventually".