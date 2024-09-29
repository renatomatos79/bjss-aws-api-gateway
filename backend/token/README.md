# Building and Deploy a Docker Image login-backend

```sh
docker build -t login-backend:8.0.1 .
docker tag login-backend:8.0.1 renatomatos79/login-backend:8.0.1
docker login
docker push renatomatos79/login-backend:8.0.1
docker run -d --name login-backend-8.0.1 --restart unless-stopped -p 8080:8080 renatomatos79/login-backend:8.0.1
docker container logs login-backend
```

