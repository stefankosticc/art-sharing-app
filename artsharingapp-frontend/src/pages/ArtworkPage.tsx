import Dock from "../components/Dock";
import "../styles/ArtworkPage.css";
import { useArtwork } from "../hooks/useArtwork";
import { PiDotsThreeOutlineVerticalFill } from "react-icons/pi";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import { MdEdit } from "react-icons/md";
import { IoMdHeartEmpty, IoMdHeart } from "react-icons/io";
import { useEffect, useState } from "react";
import { dislikeArtwork, likeArtwork } from "../services/artwork";
import { useParams } from "react-router-dom";

const ArtworkPage = () => {
  const { artworkId } = useParams();

  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [isLiked, setIsLiked] = useState<boolean>(false);
  const fallbackImage =
    "https://cdn.shopify.com/s/files/1/0047/4231/6066/files/The_Scream_by_Edvard_Munch_1893_800x.png";
  // "https://www.theartist.me/wp-content/uploads/2021/02/famous-micheal-angeleo-paintings.jpg";
  // "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500";

  const { artwork } = useArtwork(artworkId ? parseInt(artworkId) : -1);
  const { loggedInUser } = useLoggedInUser();

  const [imgSrc, setImgSrc] = useState<string>("");

  useEffect(() => {
    if (artwork?.image) {
      setImgSrc(artwork.image);
    }
    if (artwork?.isLikedByLoggedInUser) {
      setIsLiked(artwork.isLikedByLoggedInUser);
    }
  }, [artwork]);

  return (
    <div className="artwork-page fixed-page">
      <div className="ap-image-container">
        {imgSrc ? (
          <img
            src={imgSrc}
            alt={artwork?.title || "artwork image"}
            className="ap-image"
            onError={() => setImgSrc(fallbackImage)}
          />
        ) : (
          <div className="ap-image ap-image-placeholder skeleton"></div>
        )}
      </div>

      <div className="ap-info">
        <div className="ap-info-header">
          <h1 className="ap-title">{artwork?.title || "Artwork Title"}</h1>
          <div className="ap-info-header-right-group">
            {artwork?.isOnSale && <div className="ap-on-sale">ON SALE</div>}

            {isLiked ? (
              <IoMdHeart
                className="ap-info-header-icon"
                id="ap-liked-artwork-icon"
                title="Dislike"
                onClick={async () => {
                  if (artwork) {
                    await dislikeArtwork(artwork.id);
                    setIsLiked(false);
                  }
                }}
              />
            ) : (
              <IoMdHeartEmpty
                className="ap-info-header-icon"
                title="Like"
                onClick={async () => {
                  if (artwork) {
                    setIsLiked(await likeArtwork(artwork.id));
                  }
                }}
              />
            )}

            {artwork?.postedByUserId === loggedInUser?.id && (
              <div className="ap-info-header-right-group">
                <MdEdit
                  className="ap-info-header-icon"
                  title="Edit"
                  onClick={() => setIsEditing(!isEditing)}
                />
                <PiDotsThreeOutlineVerticalFill
                  className="ap-info-header-icon"
                  id="ap-3-dots-menu"
                  title="Options"
                />
              </div>
            )}
          </div>
        </div>

        <div className="ap-details">
          <div className="ap-date">
            <p className="ap-details-label">DATE</p>
            <p className="ap-details-text">
              {artwork?.date.toString() || "XX-XX-XXXX"}
            </p>
          </div>
          <div className="ap-user-profile">
            <p className="ap-details-label">CREATED BY</p>
            <div className="ap-user-info">
              <img
                src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"
                alt="Default profile picture"
                className="ap-user-profile-picture"
              />
              <span className="ap-details-text">@createdby</span>
            </div>
          </div>
          <div className="ap-user-profile">
            <p className="ap-details-label">POSTED BY</p>
            <div className="ap-user-info">
              <img
                src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"
                alt="Default profile picture"
                className="ap-user-profile-picture"
              />
              <span className="ap-details-text">@postedby</span>
            </div>
          </div>
        </div>

        <p className="ap-story">
          Lorem ipsum dolor sit amet consectetur adipisicing elit. Nisi deleniti
          vel expedita tenetur nulla explicabo in dolorem laudantium nam veniam
          voluptates, libero vero quia laboriosam cupiditate eius illo!
          Dignissimos, neque. Iure enim sunt quia, facere magnam mollitia minima
          hic, doloremque saepe doloribus tenetur quasi soluta corporis eius.
          Doloremque exercitationem architecto, consequatur provident labore
          laboriosam veritatis libero nesciunt, blanditiis atque ad! Distinctio
          repellendus mollitia suscipit ipsum voluptas magni repellat aut esse
          excepturi dicta voluptatibus, iste in voluptate doloribus cum, eum
          accusamus. <br />
          Tempore fugiat, ex aspernatur iste molestias unde amet eos qui.
          Placeat quae ducimus delectus tempora officia porro maiores iusto est
          omnis provident, quis commodi, quos sequi sed laboriosam! Minima ut
          reiciendis cum aut consequuntur assumenda libero hic nam mollitia
          earum. Quis optio suscipit libero maxime debitis distinctio accusamus
          veritatis vero, qui dolore consequatur saepe, illum itaque expedita
          deserunt. Similique, aperiam molestias doloremque esse est
          perspiciatis harum in placeat totam quis! <br />
          Recusandae, sint fugiat voluptatem adipisci facilis eligendi dolores
          possimus, inventore distinctio error, reprehenderit quaerat suscipit
          laborum ea blanditiis eaque ducimus dolorem architecto saepe ipsa
          dignissimos. Laborum porro doloremque distinctio consequuntur.
          Mollitia, illo iste consectetur blanditiis qui cumque quo sed harum
          repudiandae, consequuntur sit doloribus ratione eveniet sunt
          reiciendis eum repellendus similique quod fugit commodi!br At hic iure
          blanditiis debitis sint. <br />
          Error, doloribus impedit sapiente, dicta nostrum obcaecati vitae
          laboriosam libero nemo dolores tempora sed nisi, corporis delectus.
          Minus sunt dignissimos assumenda quidem repellendus rerum possimus
          quod sequi. Cupiditate, amet quia. Nam pariatur voluptatum natus quam
          harum quisquam, quibusdam praesentium temporibus necessitatibus.
          Deserunt repellat sequi, non placeat modi officiis! Ipsa perspiciatis
          placeat culpa labore eos magni! <br /> Commodi necessitatibus
          reprehenderit non eligendi. Maxime, deserunt natus animi excepturi
          suscipit nam iste sapiente molestiae, fugiat mollitia tenetur adipisci
          quos voluptatibus! Nemo, quis rem! Aut expedita delectus a nihil
          accusamus quam, similique repudiandae eius. Nemo?
        </p>
        <div className="ap-tips-and-tricks">
          <h3>Tips and Tricks</h3>
          Lorem ipsum, dolor sit amet consectetur adipisicing elit. Asperiores
          eum sed optio. Molestias ullam numquam inventore quibusdam accusamus
          voluptatum omnis eum possimus laborum repudiandae quam officia
          consequatur, alias quod repellendus. Illum quae fuga recusandae
          officiis sed omnis error eveniet, neque sit facere repudiandae illo id
          doloribus praesentium sint aliquid?
          <br /> Incidunt alias beatae repellendus ea dolor provident ab
          voluptatum necessitatibus saepe? Laborum incidunt vitae maiores nisi
          laudantium ab ipsam quidem voluptates non omnis! Repellat id, commodi,
          animi maiores totam illum ex, assumenda consequatur sunt alias quia
          nam quas odit soluta dicta. Praesentium earum non laboriosam inventore
          voluptates. <br /> Distinctio, ullam aperiam. Aliquam eaque aperiam
          ipsa eveniet magnam vero explicabo magni veritatis voluptas, earum
          inventore, fugit provident itaque rem commodi omnis molestias
          adipisci. Lorem ipsum, dolor sit amet consectetur adipisicing elit.
          Asperiores eum sed optio. Molestias ullam numquam inventore quibusdam
          accusamus voluptatum omnis eum possimus laborum repudiandae quam
          officia consequatur, alias quod repellendus. Illum quae fuga
          recusandae officiis sed omnis error eveniet, neque sit facere
          repudiandae illo id doloribus praesentium sint aliquid?
          <br /> Incidunt alias beatae repellendus ea dolor provident ab
          voluptatum necessitatibus saepe? Laborum incidunt vitae maiores nisi
          laudantium ab ipsam quidem voluptates non omnis! Repellat id, commodi,
          animi maiores totam illum ex, assumenda consequatur sunt alias quia
          nam quas odit soluta dicta. Praesentium earum non laboriosam inventore
          voluptates. <br /> Distinctio, ullam aperiam. Aliquam eaque aperiam
          ipsa eveniet magnam vero explicabo magni veritatis voluptas, earum
          inventore, fugit provident itaque rem commodi omnis molestias
          adipisci.
        </div>

        {isEditing && (
          <div className="ap-info-edit-btns">
            <button
              onClick={() => setIsEditing(false)}
              className="ap-cancel-edit-btn"
            >
              Cancel
            </button>
            <button className="ap-save-edit-btn">Save</button>
          </div>
        )}
      </div>

      <Dock />
    </div>
  );
};

export default ArtworkPage;
