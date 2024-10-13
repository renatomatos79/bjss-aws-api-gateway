# Create a security group to allow SSH access
resource "aws_security_group" "udemy_ec2_sg" {
  name        = "udemy_ec2_sg"
  description = "Security group for custom TCP, SSH, RDP and HTTPS access"
  vpc_id      = var.vpc_id

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
    cidr_blocks = [var.my_ip]  
  }

  # Ingress rule for RDP (port 3389) from a specific IP address
  ingress {
    from_port   = 3389
    to_port     = 3389
    protocol    = "tcp"
    cidr_blocks = [var.my_ip]  
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