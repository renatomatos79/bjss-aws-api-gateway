export async function handler (event, context ) {

  console.log('Received event:', JSON.stringify(event));

  // from header, extracts the authorization code and source IP
  const authorization = event.headers['Authorization']
  const sourceIp = event.requestContext.identity.sourceIp ?? ''
  const authCode = getCredentialContent(authorization) ?? ''
  
  try {
    console.log("authorization: ", authorization)
    console.log("sourceIp: ", sourceIp)
    console.log("authCode: ", authCode)

    // auth code must be available
    if (authCode === '') {
      throw new Error("Validation Error (authCode)")
    }
    
    if (authCode !== 'AKIA6ODU4JQHKBJW2WUH') {
      return authorizerResponse('Deny', sourceIp, authCode)
    }

    // here we are able to validate the IP
    // build your function to checkIP
    // const ipAllowed = await checkIP(sourceIp)
    // if(!ipAllowed){
    //  return authorizerResponse('Deny', sourceIp, authCode)
    // }
    
    return authorizerResponse('Allow', sourceIp, authCode)
    
  } catch (err) {
    console.log(err)
    return authorizerResponse('Deny', sourceIp, authCode)
  } 
};

// Using REGEX extracts the credential content
function getCredentialContent(header) {
  // Use regular expression to extract the credential content
  const match = header.match(/Credential=([^/]+)/);

  // Check if the match is found
  if (match && match.length > 1) {
    return match[1];
  } else {
    return null
  }
}

// build the final Policy (Allow)
function authorizerResponse (effect, sourceIp, authCode) {
  const response = {
    principalId: sourceIp,
    context: {
      "sourceIp": sourceIp,
      "authCode": authCode
    },
    policyDocument: {
        Version: '2012-10-17',
        Statement: [
            {
                Action: 'execute-api:Invoke',
                Effect: effect,
                Resource: '*',
            },
        ],
    },
  }
  return response
}
