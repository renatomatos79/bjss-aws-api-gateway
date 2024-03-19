export const handler = async (event) => {
  const breeds = [
    {
      "breed": "Fluffywoof",
      "origin": "Toy Land",
      "size": "Small",
      "temperament": ["Playful", "Affectionate", "Energetic"]
    },
    {
      "breed": "Golden Floof",
      "origin": "Sunny Meadows",
      "size": "Medium",
      "temperament": ["Friendly", "Loyal", "Intelligent"]
    },
    {
      "breed": "Snugglepup",
      "origin": "Cozy Corner",
      "size": "Small",
      "temperament": ["Cuddly", "Gentle", "Sociable"]
    },
    {
      "breed": "Thunderhound",
      "origin": "Stormy Mountains",
      "size": "Large",
      "temperament": ["Brave", "Strong", "Protective"]
    },
    {
      "breed": "Spritelytail",
      "origin": "Enchanted Forest",
      "size": "Medium",
      "temperament": ["Playful", "Curious", "Agile"]
    }
  ]
  
  // TODO implement
  const response = {
    statusCode: 200,
    body: JSON.stringify(breeds),
  };
  return response;
};
