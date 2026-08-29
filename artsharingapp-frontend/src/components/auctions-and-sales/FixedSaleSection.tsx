import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Artwork } from "../../services/artwork";
import { Currency } from "../../services/enums";
import "./styles/AuctionSection.css";
import "./styles/FixedSaleSection.css";
import { AiOutlineSend } from "react-icons/ai";

type FixedSaleSectionProps = {
  artwork: Artwork;
};

const FixedSaleSection = ({ artwork }: FixedSaleSectionProps) => {
  const { t } = useTranslation();
  const navigation = useNavigate();

  return (
    <div className="auction-section-container fixed-sale-section-container">
      <h4>{t("auctions.onSaleHeading")}</h4>
      <hr />
      <div className="auction-section-content">
        <div className="auction-section-column">
          <p>{t("auctions.price")}</p>
          <p className="auction-section-column-info">
            {artwork.price?.toLocaleString("en-US")}{" "}
            {Currency[artwork.currency]}
          </p>
        </div>
        <div className="auction-section-column">
          <p>{t("chat.sendMessage")}</p>
          <div className="auction-section-offer fixed-sale-section-message">
            <button
              title={t("chat.sendMessage")}
              onClick={() =>
                navigation("/chat", {
                  state: {
                    selectedUser: {
                      userId: artwork.postedByUserId,
                      userName: artwork.postedByUserName,
                      profilePhoto: artwork.postedByUserProfilePhoto,
                    },
                    input: t("auctions.interestedMessage", {
                      title: artwork.title,
                    }),
                  },
                })
              }
            >
              <AiOutlineSend />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default FixedSaleSection;
