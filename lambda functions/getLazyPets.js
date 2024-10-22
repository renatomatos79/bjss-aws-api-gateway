function sleep(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

export const handler = async (event) => {
  await sleep(5000);
  
  const pets = [
    { petId: 1, name: 'Buddy', type: 'Dog', age: 3 },
    { petId: 2, name: 'Mittens', type: 'Cat', age: 5 },
    { petId: 3, name: 'Goldie', type: 'Fish', age: 1 },
    { petId: 4, name: 'Shadow', type: 'Dog', age: 7 },
  ];
  
  try {
    // Get the petId from the query parameters
    const petId = parseInt(event.queryStringParameters.petId);

    // Validate the petId
    if (!petId) {
      return {
        statusCode: 400,
        body: JSON.stringify({ message: 'petId is required' }),
      };
    }

    // Find the pet that matches the given petId
    const pet = pets.find((p) => p.petId === petId);

    // If no pet is found, return a 404 response
    if (!pet) {
      return {
        statusCode: 404,
        body: JSON.stringify({ message: 'Pet not found' }),
      };
    }

    // Return the found pet
    return {
      statusCode: 200,
      body: JSON.stringify(pet),
    };
  } catch (error) {
    // Handle any errors
    return {
      statusCode: 500,
      body: JSON.stringify({ message: 'Internal Server Error', error: error.message }),
    };
  }
};
