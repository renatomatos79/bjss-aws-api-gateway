# Input variables
variable "my_ip" {
  description = "The public IP to allow SSH access to the EC2 instance"
  type        = string
}

variable "aws_region" {
  description = "The AWS region to deploy the resources"
  type        = string
  default     = "us-east-1"
}

variable "instance_type" {
  description = "EC2 instance type"
  type        = string
}

variable "ec2_deploy_ami_instance" {
  description = "ID used to deploy the EC2 instance"
  type        = string
}

variable "ec2_key_name" {
  description = "Key pair name to access the EC2 instance via SSH"
  type        = string
}

variable "vpc_id" {
  description = "ID of the VPC to deploy the resources"
  type        = string
}