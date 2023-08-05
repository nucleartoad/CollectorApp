
# CollectorApp

####

this is a simple collector webapp that features the ability to track anything that you may want to collect.

##### features:

- login and authentication
- creation of collections
- adding items to collections
- deleting collections and items

###### TODO:

- feature to add images to collections and/or items

## Preparing the application

### backend

install the dotnet dependencies

```bash
$ dotnet restore
```

prepare the sqlite database

```bash
$ dotnet ef migrations add
$ dotnet ef database update
```

### frontend

first cd into ClientApp/collector

```bash
$ cd ClientApp/collector
```

install the npm dependencies

```bash
$ npm install
```

## Running the application

first start the dotnet backend from the root folder

```bash
$ dotnet run
```

then cd into the Client/App folder using a seperate terminal
start the react frontend

```bash
$ npm start
```
