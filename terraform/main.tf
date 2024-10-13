# Create an EC2 instance
resource "aws_instance" "udemy_docker_deploy" {
  ami           = var.ec2_deploy_ami_instance
  instance_type = var.instance_type

  # Optional: Add a key pair to access the instance via SSH
  key_name = var.ec2_key_name

  # Tags (optional)
  tags = {
    Name = "udemy-docker-deploy"
    Groups = "udemy"
  }

  # Optional: Security group to allow SSH access (port 22)
  vpc_security_group_ids = [aws_security_group.udemy_ec2_sg.id]
}

