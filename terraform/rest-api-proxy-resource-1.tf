#####################################
# Proxy Resource 1: api-events/{proxy+}
#####################################

# Create the /api-events resource
resource "aws_api_gateway_resource" "api_events" {
  rest_api_id = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  parent_id   = aws_api_gateway_rest_api.udemy_proxy_api_gateway.root_resource_id
  path_part   = "api-events"
}

# Create the /api-events/{proxy+} child resource
resource "aws_api_gateway_resource" "api_events_proxy" {
  rest_api_id = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  parent_id   = aws_api_gateway_resource.api_events.id
  path_part   = "{proxy+}"  # This declares the proxy path
}

# Define the ANY method for /api-events/{proxy+}
resource "aws_api_gateway_method" "api_events_any_method" {
  rest_api_id   = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  resource_id   = aws_api_gateway_resource.api_events_proxy.id
  http_method   = "ANY"
  authorization = "NONE"

  # Define the required headers
  request_parameters = {
    "method.request.header.Authorization" = true
    "method.request.header.Content-Type"  = true
  }
}

# Integration for the ANY method on /api-events/{proxy+}
resource "aws_api_gateway_integration" "api_events_integration" {
  rest_api_id             = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  resource_id             = aws_api_gateway_resource.api_events_proxy.id
  http_method             = aws_api_gateway_method.api_events_any_method.http_method
  integration_http_method = "ANY"
  type                    = "HTTP"
  uri                     = "http://44.196.119.69:8083/api/{proxy}"

  # Correctly map the request headers and path to the backend
  request_parameters = {
    "integration.request.header.Authorization" = "method.request.header.Authorization"
    "integration.request.header.Content-Type"  = "method.request.header.Content-Type"
  }

  # Pass through all the remaining client requests
  passthrough_behavior = "WHEN_NO_TEMPLATES"
}
