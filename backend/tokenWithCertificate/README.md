# Building and Deploy a Docker Image login-backend

```sh
docker build -t login-cert-backend:8.0.1 .
docker tag login-cert-backend:8.0.1 renatomatos79/login-cert-backend:8.0.1
docker login
docker push renatomatos79/login-cert-backend:8.0.1
docker run -d --name login-cert-backend-8.0.1 --restart unless-stopped -p 443:443 renatomatos79/login-cert-backend:8.0.1
docker container logs login-cert-backend
```

