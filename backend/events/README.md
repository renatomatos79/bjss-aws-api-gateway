# Creating a Docker Network

```sh
docker network create --driver bridge backend-bridge-network
```

# Starting redis

```sh
docker rm redisserver
docker run --name redisserver -d --network=backend-bridge-network -p 6379:6379 redis
```

# Building and Deploy a Docker Image events-backend

```sh
docker build -t events-backend:8.0.2 .
docker tag events-backend:8.0.2 renatomatos79/events-backend:8.0.2
docker login
docker push renatomatos79/events-backend:8.0.2
docker run -d --network=backend-bridge-network --name events-backend-8.0.2 -p 8081:8080 renatomatos79/events-backend:8.0.2
docker container logs events-backend
```

# Running backend using Amazon Elastic Cache from ECR

```sh
docker container rm events-backend-8.0.1 --force
docker run -d --name events-backend-8.0.1 -e REDIS_HOST_TYPE=REDIS_AMAZON_OSS -e REDIS_HOST='clustercfg.awsredis.uzxmqs.use1.cache.amazonaws.com:6379' -p 8086:8080 992382569486.dkr.ecr.us-east-1.amazonaws.com/aws-udemy:events-backend-8.0.1
docker container logs events-backend-8.0.1
```

# event list

```sh
{
  "timeout": 1200,
  "events": 
  [
        {
            "Id": 1,
            "Title": "Introduction to Artificial Intelligence",
            "Content": "Join us for an overview of AI concepts and applications.",
            "Speaker": "Dr. John Smith",
            "Date": "2024-04-10T10:00:00"
        },
        {
            "Id": 2,
            "Title": "Machine Learning Workshop",
            "Content": "Hands-on workshop covering various machine learning algorithms.",
            "Speaker": "Dr. Emily Johnson",
            "Date": "2024-04-15T13:30:00"
        },
        {
            "Id": 3,
            "Title": "Blockchain Technology: Beyond Cryptocurrency",
            "Content": "Exploring the potential of blockchain in various industries.",
            "Speaker": "Mr. Alex Brown",
            "Date": "2024-04-20T11:00:00"
        },
        {
            "Id": 4,
            "Title": "Cybersecurity Best Practices",
            "Content": "Learn about the latest cybersecurity threats and how to mitigate them.",
            "Speaker": "Dr. Sarah Lee",
            "Date": "2024-04-25T14:00:00"
        },
        {
            "Id": 5,
            "Title": "Big Data Analytics Symposium",
            "Content": "Bringing together experts to discuss the challenges and opportunities in big data analytics.",
            "Speaker": "Dr. Michael Wang",
            "Date": "2024-05-05T09:30:00"
        },
        {
            "Id": 6,
            "Title": "Internet of Things (IoT) Innovation Summit",
            "Content": "Explore the latest innovations and trends in the Internet of Things space.",
            "Speaker": "Mr. James Adams",
            "Date": "2024-05-10T10:30:00"
        },
        {
            "Id": 7,
            "Title": "Ethical AI: Principles and Practices",
            "Content": "Discussing ethical considerations in the development and deployment of AI systems.",
            "Speaker": "Dr. Emily Johnson",
            "Date": "2024-05-15T13:00:00"
        },
        {
            "Id": 8,
            "Title": "Future of Work: Adapting to Technological Changes",
            "Content": "Examining how technology is reshaping the workplace and strategies for adaptation.",
            "Speaker": "Ms. Sophia Miller",
            "Date": "2024-05-20T11:30:00"
        },
        {
            "Id": 9,
            "Title": "Quantum Computing: Breaking the Boundaries",
            "Content": "An overview of quantum computing principles and its potential applications.",
            "Speaker": "Dr. David Chen",
            "Date": "2024-05-25T14:30:00"
        },
        {
            "Id": 10,
            "Title": "Augmented Reality in Education",
            "Content": "Exploring how AR technology can enhance learning experiences.",
            "Speaker": "Mr. Alex Brown",
            "Date": "2024-06-05T09:00:00"
        },
        {
            "Id": 11,
            "Title": "Data Privacy: Challenges and Solutions",
            "Content": "Discussing the importance of data privacy and strategies for protecting personal information.",
            "Speaker": "Dr. Sarah Lee",
            "Date": "2024-06-10T10:00:00"
        },
        {
            "Id": 12,
            "Title": "Deep Learning Applications in Healthcare",
            "Content": "Exploring how deep learning is revolutionizing healthcare delivery and patient outcomes.",
            "Speaker": "Dr. Michael Wang",
            "Date": "2024-06-15T13:45:00"
        },
        {
            "Id": 13,
            "Title": "Smart Cities: Building Sustainable Urban Environments",
            "Content": "Examining the role of technology in creating smarter and more sustainable cities.",
            "Speaker": "Mr. James Adams",
            "Date": "2024-06-20T11:15:00"
        },
        {
            "Id": 14,
            "Title": "Artificial General Intelligence: The Next Frontier",
            "Content": "A discussion on the future prospects and challenges of achieving AGI.",
            "Speaker": "Dr. John Smith",
            "Date": "2024-06-25T14:15:00"
        },
        {
            "Id": 15,
            "Title": "Cloud Computing: Trends and Innovations",
            "Content": "An overview of the latest trends and innovations in cloud computing.",
            "Speaker": "Ms. Sophia Miller",
            "Date": "2024-07-05T09:45:00"
        },
        {
            "Id": 16,
            "Title": "Robotics in Industry 4.0",
            "Content": "Exploring the role of robotics in the fourth industrial revolution.",
            "Speaker": "Dr. David Chen",
            "Date": "2024-07-10T10:45:00"
        },
        {
            "Id": 17,
            "Title": "Digital Transformation: Strategies for Success",
            "Content": "Discussing strategies for successful digital transformation initiatives.",
            "Speaker": "Dr. Sarah Lee",
            "Date": "2024-07-15T13:15:00"
        },
        {
            "Id": 18,
            "Title": "5G Technology and its Impact",
            "Content": "Examining the potential of 5G technology to transform various industries.",
            "Speaker": "Mr. Alex Brown",
            "Date": "2024-07-20T11:45:00"
        },
        {
            "Id": 19,
            "Title": "The Future of Transportation: Innovations and Challenges",
            "Content": "Exploring emerging technologies and challenges in the transportation sector.",
            "Speaker": "Dr. Emily Johnson",
            "Date": "2024-07-25T14:45:00"
        },
        {
            "Id": 20,
            "Title": "Artificial Intelligence in Finance",
            "Content": "An overview of AI applications in the finance industry.",
            "Speaker": "Dr. John Smith",
            "Date": "2024-08-05T09:15:00"
        }
    ]
}
```
