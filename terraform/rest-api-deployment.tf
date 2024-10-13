#####################################
# API Deployment
#####################################

# Deploy the API to a stage
resource "aws_api_gateway_deployment" "api_deployment" {
  depends_on = [
    aws_api_gateway_integration.api_events_integration,
    aws_api_gateway_integration.api_token_integration
  ]

  rest_api_id = aws_api_gateway_rest_api.udemy_proxy_api_gateway.id
  stage_name  = "dev-v1"
}

