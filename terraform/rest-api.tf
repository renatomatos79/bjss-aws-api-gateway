# Create the API Gateway (regional)
resource "aws_api_gateway_rest_api" "udemy_proxy_api_gateway" {
  name        = "udemy_proxy_api_gateway"
  description = "API Gateway with two proxy resources"
  endpoint_configuration {
    types = ["REGIONAL"]
  }
   # Tags (optional)
  tags = {
    Name = "udemy_proxy_api_gateway"
    Groups = "udemy"
  }
}