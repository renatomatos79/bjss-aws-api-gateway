export async function handler (event, context) {

  console.log('Received event:', event);
  console.log('Received context:', context);
  
  let accessKey = ''

  
  try {
    const authorizationToken = event.headers['Authorization']
	
	// authorization token must be valid
    if (!authorizationToken) {
      throw new Error("Validation Error (authorizationToken)")
    }
	
	// print authorizationToken
	console.log("authorizationToken: ", authorizationToken)
	
    // extracts the accessKey from authorization token
    accessKey = getCredentialKey(authorizationToken) ?? ''

    // print accessKey
    console.log("accessKey: ", accessKey)
	
	// auth code must be available
    if (accessKey === '') {
      throw new Error("Validation Error (accessKey)")
    }	
    
	// Access DB using KEY
    if (accessKey !== 'AKIA6ODU4JQHL7IVRIIM') {
      return authorizerResponse('Deny', accessKey, 'invalid access key')
    }
    
    return authorizerResponse('Allow', accessKey, '')
    
  } catch (err) {
    console.log(err)
    return authorizerResponse('Deny', accessKey, err)
  } 
};

// Using REGEX extracts the credential content
function getCredentialKey(header) {
  // Use regular expression to extract the credential content
  const match = header.match(/Credential=([^/]+)/);

  // Check if the match is found
  if (match && match.length > 1) {
    return match[1];
  } else {
    return null
  }
}

// create a UUID
function generateUUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        const r = Math.random() * 16 | 0;
        const v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

// build the final Policy (Allow)
function authorizerResponse(effect, accessKey, msg) {
  const response = {
    principalId: generateUUID(),
    context: {
      "msg": msg,
      "accessKey": accessKey
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
