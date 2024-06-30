const express = require("express");
const cors = require("cors");
const bodyparser = require("body-parser");

const app = express();
app.use(express.static("public"));
app.use(bodyparser.urlencoded({ extended: false }));
app.use(bodyparser.json());
app.use(cors({ origin: true, credentials: true }));

const stripe = require("stripe")("sk_test_51LUQwuG8zN8FyI0EjWfNxqrEkwK1oDaKXTaHokFzDCryxXEvulmKO15jFFZ4ICLOeQGuzsEEMqXv14yf7Ks0wNNk00bfR4f5JR");

app.post("/checkout", async (req, res, next) => {
    try {
        const session = await stripe.checkout.sessions.create({
       /* payment_method_types: ['card'],
        shipping_address_collection: {
        allowed_countries: ['US', 'CA'],
        },
            shipping_options: [
            {
                shipping_rate_data: {
                type: 'fixed_amount',
                fixed_amount: {
                    amount: 0,
                    currency: 'usd',
                },
                display_name: 'Free shipping',
                // Delivers between 5-7 business days
                delivery_estimate: {
                    minimum: {
                    unit: 'business_day',
                    value: 5,
                    },
                    maximum: {
                    unit: 'business_day',
                    value: 7,
                    },
                }
                }
            },
            {
                shipping_rate_data: {
                type: 'fixed_amount',
                fixed_amount: {
                    amount: 1500,
                    currency: 'usd',
                },
                display_name: 'Next day air',
                // Delivers in exactly 1 business day
                delivery_estimate: {
                    minimum: {
                    unit: 'business_day',
                    value: 1,
                    },
                    maximum: {
                    unit: 'business_day',
                    value: 1,
                    },
                }
                }
            },
            ],*/
           line_items:  req.body.items.map((item) => ({
            price_data: {
              currency: 'rsd',
              product_data: {
                name: item.naziv_knjige,
                images: [item.slika]
              },
              unit_amount: item.ukupna_cena,
            },
            quantity: item.quantity * 100,
          })),
           mode: "payment",
           success_url: "http://localhost:4242/success.html",
           cancel_url: "http://localhost:4242/cancel.html",
        });

        res.status(200).json(session);
    } catch (error) {
        next(error);
    }
});

app.post('/webhook', async (req, res) => {
    const sig = req.headers['stripe-signature'];

    let event;

    try {
        event = stripe.webhooks.constructEvent(req.body, sig, endpointSecret);
    } catch (err) {
        res.status(400).send(`Webhook Error: ${err.message}`);
        return;
    }

    // Obrada događaja
    if (event.type === 'checkout.session.completed') {
        
        const session = event.data.object;
        // Uspešno plaćanje
        // Kreirajte porudžbinu ovde
        
        // Na primer, pozovite funkciju za kreiranje porudžbine
        createOrder(session);
       
    } else {
        console.log(`Unhandled event type ${event.type}`);
    }

    res.status(200).end();
});

function createOrder(session) {
    // Logika za kreiranje porudžbine na osnovu podataka iz sesije
    console.log('Order created for session:', session);
}


app.listen(4242, () => console.log('app is running on 4242'));
