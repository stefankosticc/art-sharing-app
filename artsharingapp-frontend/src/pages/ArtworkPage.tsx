import Dock from "../components/Dock";
import "../styles/ArtworkPage.css";
import { useArtwork } from "../hooks/useArtwork";
import { PiDotsThreeOutlineVerticalFill } from "react-icons/pi";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import { MdEdit } from "react-icons/md";
import { IoMdHeartEmpty, IoMdHeart } from "react-icons/io";
import { useEffect, useState } from "react";
import {
  ArtworkRequest,
  dislikeArtwork,
  likeArtwork,
  updateArtwork,
} from "../services/artwork";
import { useParams } from "react-router-dom";
import TextEditor from "../components/TextEditor";

const ArtworkPage = () => {
  const { artworkId } = useParams();

  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [isLiked, setIsLiked] = useState<boolean>(false);

  const fallbackImage =
    "https://cdn.shopify.com/s/files/1/0047/4231/6066/files/The_Scream_by_Edvard_Munch_1893_800x.png";
  // "https://www.theartist.me/wp-content/uploads/2021/02/famous-micheal-angeleo-paintings.jpg";
  // "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500";

  const [refetchArtwork, setRefetchArtwork] = useState<boolean>(false);
  const { artwork } = useArtwork(
    artworkId ? parseInt(artworkId) : -1,
    refetchArtwork
  );
  const { loggedInUser } = useLoggedInUser();

  const [imgSrc, setImgSrc] = useState<string>("");

  const [editingArtworkData, setEditingArtworkData] = useState<ArtworkRequest>({
    title: "",
    story: "",
    image: "",
    date: new Date(),
    tipsAndTricks: "",
    isPrivate: false,
    createdByArtistId: 0,
    postedByUserId: 0,
    cityId: null,
    galleryId: null,
  });

  useEffect(() => {
    if (artwork?.image) {
      setImgSrc(artwork.image);
    }
    if (artwork?.isLikedByLoggedInUser) {
      setIsLiked(artwork.isLikedByLoggedInUser);
    }
  }, [artwork]);

  useEffect(() => {
    if (isEditing && artwork) {
      setEditingArtworkData({
        title: artwork.title || "",
        story: artwork.story || "",
        image: artwork.image || "",
        date: artwork.date || new Date(),
        tipsAndTricks: artwork.tipsAndTricks || "",
        isPrivate: artwork.isPrivate || false,
        createdByArtistId: artwork.createdByArtistId || -1,
        postedByUserId: artwork.postedByUserId || -1,
        cityId: artwork.cityId || null,
        galleryId: artwork.galleryId || null,
      });
    }
  }, [isEditing, artwork]);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => {
    const { name, value } = e.target;
    setEditingArtworkData((prev) => ({ ...prev, [name]: value }));
  };

  const handleStoryChange = ({ editor }: { editor: any }) => {
    setEditingArtworkData((prev) => ({
      ...prev,
      story: editor.getHTML(),
    }));
  };

  const handleTipsChange = ({ editor }: { editor: any }) => {
    setEditingArtworkData((prev) => ({
      ...prev,
      tipsAndTricks: editor.getHTML(),
    }));
  };

  const handleCancelEdit = () => {
    setIsEditing(false);
    if (artwork) {
      setEditingArtworkData({
        title: artwork.title || "",
        story: artwork.story || "",
        image: artwork.image || "",
        date: artwork.date || new Date(),
        tipsAndTricks: artwork.tipsAndTricks || "",
        isPrivate: artwork.isPrivate || false,
        createdByArtistId: artwork.createdByArtistId || 0,
        postedByUserId: artwork.postedByUserId || 0,
        cityId: artwork.cityId || null,
        galleryId: artwork.galleryId || null,
      });
    }
  };

  const handleSaveEdit = async () => {
    if (!editingArtworkData.title.trim()) {
      alert("Title is required.");
      return;
    }

    if (editingArtworkData.title.length > 100) {
      alert("Title must be under 100 characters.");
      return;
    }

    if (artwork) {
      await updateArtwork(artwork.id, editingArtworkData);
      setRefetchArtwork(!refetchArtwork);
      setIsEditing(false);
    }
  };

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
          {isEditing ? (
            <textarea
              className="ap-title ap-title-editing"
              name="title"
              value={editingArtworkData.title}
              onChange={handleInputChange}
              rows={1}
              maxLength={100}
            />
          ) : (
            <h1 className="ap-title">{artwork?.title || ""}</h1>
          )}
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
              {artwork?.date?.toString() || "-"}
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
              <span className="ap-details-text">
                {artwork?.createdByArtistUserName
                  ? "@" + artwork.createdByArtistUserName
                  : "-"}
              </span>
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
              <span className="ap-details-text">
                {artwork?.postedByUserName
                  ? "@" + artwork.postedByUserName
                  : "-"}
              </span>
            </div>
          </div>
        </div>

        {((artwork?.story && artwork.story !== "<p></p>") || isEditing) && (
          <div className="ap-story">
            <h3>Story</h3>
            <TextEditor
              content={isEditing ? editingArtworkData.story : artwork?.story}
              editable={isEditing}
              className={`ap-text-editor ${
                isEditing ? "text-editor-editing" : ""
              }`}
              onUpdate={isEditing ? handleStoryChange : undefined}
              disableHeadings={true}
            />
          </div>
        )}

        {((artwork?.tipsAndTricks && artwork.tipsAndTricks !== "<p></p>") ||
          isEditing) && (
          <div className="ap-tips-and-tricks">
            <h3>Tips and Tricks</h3>
            <TextEditor
              content={
                isEditing
                  ? editingArtworkData.tipsAndTricks
                  : artwork?.tipsAndTricks
              }
              editable={isEditing}
              className={`ap-text-editor ${
                isEditing ? "text-editor-editing" : ""
              }`}
              onUpdate={isEditing ? handleTipsChange : undefined}
              disableHeadings={true}
            />
          </div>
        )}

        {isEditing && (
          <div className="ap-info-edit-btns">
            <button
              onClick={handleCancelEdit}
              className="ap-cancel-edit-btn"
              type="button"
            >
              Cancel
            </button>
            <button
              className="ap-save-edit-btn"
              onClick={handleSaveEdit}
              type="button"
            >
              Save
            </button>
          </div>
        )}
      </div>

      <Dock />
    </div>
  );
};

export default ArtworkPage;
