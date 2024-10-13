# Define the AWS provider with the desired region
provider "aws" {
  region = "us-east-1"  # Replace with your desired region
}

# Create a security group to allow SSH access
resource "aws_security_group" "udemy_ec2_sg" {
  name        = "udemy_ec2_sg"
  description = "Security group for custom TCP, SSH, RDP and HTTPS access"
  vpc_id      = "vpc-02060c972ff3b327b"

  # Ingress rule for custom TCP traffic (port range 8080-8083) from any IP
  ingress {
    from_port   = 8080
    to_port     = 8083
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Ingress rule for SSH (port 22) from a specific IP address
  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["89.180.236.241/32"]  # Replace with your specific IP
  }

  # Ingress rule for RDP (port 3389) from a specific IP address
  ingress {
    from_port   = 3389
    to_port     = 3389
    protocol    = "tcp"
    cidr_blocks = ["89.180.236.241/32"]  # Replace with your specific IP
  }

  # Ingress rule for HTTPS (port 443) from any IP
  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Egress rule to allow all outbound traffic
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"  # "-1" means all protocols
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Create an EC2 instance
resource "aws_instance" "udemy-docker-deploy" {
  ami           = "ami-0a73cb27671a8277c"  # The AMI ID you provided
  instance_type = "t2.micro"  # Instance type (you can change it as needed)

  # Optional: Add a key pair to access the instance via SSH
  key_name = "udemy-aws-hosting-strategy.pem"  # Replace with your actual key pair name

  # Tags (optional)
  tags = {
    Name = "udemy-docker-deploy"
    Groups = "udemy"
  }

  # Optional: Security group to allow SSH access (port 22)
  vpc_security_group_ids = [aws_security_group.udemy_ec2_sg.id]
}

