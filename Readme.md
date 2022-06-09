# Readme

![Case](docs/case.png)

# Docker

Build Image
```ps
docker build -f "${fullpath}\RestfulApiExample\Dockerfile" --force-rm -t restfulapiexample "${fullpath}\restful-api-example"
```

Run Image
```ps
docker run -d -p 8080:80 --name restfulapi restfulapiexample
```
