function main() {
	console.log("requesting breeds")
	$.ajax({
	  url: "https://1k9ggr35e1.execute-api.us-east-1.amazonaws.com/dev/pets/breeds",
	  success: function(data) {
		console.log(data)
	  }
	})
}

