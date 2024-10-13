# Outputs
output "security_group_id" {
  description = "The ID of the created security group"
  value       = aws_security_group.udemy_ec2_sg.id
}

output "ec2_deploy_id" {
  description = "The ID used to reference the EC2 instance"
  value       = aws_instance.udemy_docker_deploy.id
}