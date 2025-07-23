import "./styles/Features.css";

const featuresData: {
  title: string;
  image: string;
  description: string;
}[] = [
  {
    title: "Share your work",
    image:
      "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500",
    description: "Showcase your projects and connect with a global community.",
  },
  {
    title: "Go on an adventure",
    image:
      "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500",
    description: "Explore new opportunities and discover creative challenges.",
  },
  {
    title: "Chat with others",
    image:
      "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500",
    description: "Engage in real-time discussions with like-minded creators.",
  },
  {
    title: "Participate in auctions",
    image:
      "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500",
    description: "Bid on unique items or list your own in exciting auctions.",
  },
];

const Features = () => {
  return (
    <div className="lp-features-container" id="features">
      <h2>Features</h2>
      <div className="lp-features">
        {featuresData.map((feature) => (
          <div className="feature-card" key={feature.title}>
            <div>
              <h3>{feature.title}</h3>
              <p className="feature-description">{feature.description}</p>
            </div>
            <img
              src={feature.image}
              alt="Feature Image"
              className="feature-img"
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default Features;
