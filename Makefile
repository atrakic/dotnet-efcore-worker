MAKEFLAGS += --silent

BASEDIR=$(shell git rev-parse --show-toplevel)
DB ?= db

PROJECT ?= WorkerExample.csproj

all: clean
	DOCKER_BUILDKIT=1 docker-compose up --no-color --remove-orphans -d
	docker-compose ps -a
	while ! \
		[[ "$$(docker inspect --format "{{json .State.Health }}" $(DB) | jq -r ".Status")" == "healthy" ]];\
		do \
		echo "waiting $(DB) ..."; \
		sleep 1; \
		done
	dotnet restore --use-current-runtime
	ASPNETCORE_ENVIRONMENT=Development dotnet run --project ${BASEDIR}/${PROJECT}

healthcheck:
	docker inspect $(DB) --format "{{ (index (.State.Health.Log) 0).Output }}"

sqlcmd-test:
	docker exec -it $(DB) /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$(MSSQL_SA_PASSWORD)" -b -Q "select count(*) FROM [master].[dbo].[Users]"

clean:
	dotnet clean
	docker-compose down --remove-orphans -v --rmi local

-include .env
