export const handler = async (event) => {
  const breeds = [
    {
      "name": "Labrador Retriever",
      "origin": "Canada",
      "size": "Large"
    },
    {
      "name": "German Shepherd",
      "origin": "Germany",
      "size": "Large"
    },
    {
      "name": "Golden Retriever",
      "origin": "Scotland",
      "size": "Large"
    },
    {
      "name": "Bulldog",
      "origin": "England",
      "size": "Medium"
    },
    {
      "name": "Poodle",
      "origin": "Germany/France",
      "size": "Medium"
    },
    {
      "name": "Beagle",
      "origin": "United Kingdom",
      "size": "Small"
    },
    {
      "name": "Rottweiler",
      "origin": "Germany",
      "size": "Large"
    },
    {
      "name": "Yorkshire Terrier",
      "origin": "England",
      "size": "Small"
    },
    {
      "name": "Dachshund",
      "origin": "Germany",
      "size": "Small"
    },
    {
      "name": "Boxer",
      "origin": "Germany",
      "size": "Large"
    },
    {
      "name": "Shih Tzu",
      "origin": "Tibet/China",
      "size": "Small"
    },
    {
      "name": "Siberian Husky",
      "origin": "Russia",
      "size": "Large"
    },
    {
      "name": "Doberman Pinscher",
      "origin": "Germany",
      "size": "Large"
    },
    {
      "name": "Great Dane",
      "origin": "Germany",
      "size": "Giant"
    },
    {
      "name": "Chihuahua",
      "origin": "Mexico",
      "size": "Small"
    },
    {
      "name": "Pomeranian",
      "origin": "Germany/Poland",
      "size": "Small"
    },
    {
      "name": "Maltese",
      "origin": "Mediterranean Basin",
      "size": "Small"
    },
    {
      "name": "Cocker Spaniel",
      "origin": "Spain/United Kingdom",
      "size": "Medium"
    },
    {
      "name": "Saint Bernard",
      "origin": "Switzerland",
      "size": "Giant"
    },
    {
      "name": "Border Collie",
      "origin": "United Kingdom",
      "size": "Medium"
    }
  ]

  
  // TODO implement
  const response = {
    statusCode: 200,
    body: JSON.stringify(breeds),
  };
  return response;
};
