#####################################
# Proxy Resource 2: api-token/{proxy+}
#####################################

# Create the /api-token resource
resource "aws_api_gateway_resource" "api_token" {
  rest_api_id = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  parent_id   = aws_api_gateway_rest_api.udemy_proxy_api_gateway.root_resource_id
  path_part   = "api-token"
}

# Create the /api-token/{proxy+} child resource
resource "aws_api_gateway_resource" "api_token_proxy" {
  rest_api_id = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  parent_id   = aws_api_gateway_resource.api_token.id
  path_part   = "{proxy+}"
}

# Define the ANY method for /api-token/{proxy+}
resource "aws_api_gateway_method" "api_token_any_method" {
  rest_api_id   = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  resource_id   = aws_api_gateway_resource.api_token_proxy.id
  http_method   = "ANY"
  authorization = "NONE"

  # Define the required headers
  request_parameters = {
    "method.request.header.Authorization" = true
  }
}

# Integration for the ANY method on /api-token/{proxy+}
resource "aws_api_gateway_integration" "api_token_integration" {
  rest_api_id             = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  resource_id             = aws_api_gateway_resource.api_token_proxy.id
  http_method             = aws_api_gateway_method.api_token_any_method.http_method
  integration_http_method = "ANY"
  type                    = "HTTP"
  uri                     = "http://44.196.119.69:8080/api/{proxy}"

  # Map only the Authorization header and path to the backend
  request_parameters = {
    "integration.request.header.Authorization" = "method.request.header.Authorization"
    "integration.request.path.proxy"           = "method.request.path.proxy"
  }

  passthrough_behavior = "WHEN_NO_TEMPLATES"
}