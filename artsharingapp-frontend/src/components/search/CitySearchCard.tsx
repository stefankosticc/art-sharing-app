import { FaCity } from "react-icons/fa";
import "../../styles/CitySearchCard.css";
import { City } from "../../services/city";

type CitySearchCardProps = {
  city: City;
};

const CitySearchCard = ({ city }: CitySearchCardProps) => {
  return (
    <div className="city-sc-container">
      <div className="city-sc-icon-container">
        <FaCity />
      </div>
      <div className="city-sc-details">
        <p>{city.name + ", " + city.country}</p>
      </div>
    </div>
  );
};

export default CitySearchCard;
