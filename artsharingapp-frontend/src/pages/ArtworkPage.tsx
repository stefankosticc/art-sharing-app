import Dock from "../components/Dock";
import "../styles/ArtworkPage.css";
import { useArtwork } from "../hooks/useArtwork";
import { PiDotsThreeOutlineVerticalFill } from "react-icons/pi";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import { MdEdit } from "react-icons/md";
import { IoMdHeartEmpty, IoMdHeart } from "react-icons/io";
import { useEffect, useState } from "react";
import {
  addNewArtwork,
  ArtworkRequest,
  dislikeArtwork,
  likeArtwork,
  updateArtwork,
} from "../services/artwork";
import { useNavigate, useParams } from "react-router-dom";
import TextEditor from "../components/TextEditor";

type ArtworkPageProps = {
  isNew?: boolean;
};

const fallbackImage =
  "https://cdn.shopify.com/s/files/1/0047/4231/6066/files/The_Scream_by_Edvard_Munch_1893_800x.png";

const getFormattedDateOnly = (date: Date): string => {
  return date.toISOString().split("T")[0];
};

const getInitialArtworkData = (userId: number): ArtworkRequest => ({
  title: "",
  story: "",
  image: "",
  date: getFormattedDateOnly(new Date()),
  tipsAndTricks: "",
  isPrivate: false,
  createdByArtistId: userId,
  postedByUserId: userId,
  cityId: null,
  galleryId: null,
});

const ArtworkPage = ({ isNew = false }: ArtworkPageProps) => {
  const { artworkId } = useParams();
  const navigate = useNavigate();
  const { loggedInUser } = useLoggedInUser();

  const [isEditing, setIsEditing] = useState<boolean>(isNew);
  const [isLiked, setIsLiked] = useState<boolean>(false);
  const [imgSrc, setImgSrc] = useState<string>("");
  const [refetchArtwork, setRefetchArtwork] = useState<boolean>(false);

  // Fetch artwork if not new
  const { artwork } = useArtwork(
    !isNew && artworkId ? parseInt(artworkId) : -1,
    refetchArtwork
  );

  const [editingArtworkData, setEditingArtworkData] = useState<ArtworkRequest>(
    getInitialArtworkData(loggedInUser?.id ?? -1)
  );

  useEffect(() => {
    if (artwork?.image) setImgSrc(artwork.image);
    if (artwork?.isLikedByLoggedInUser)
      setIsLiked(artwork.isLikedByLoggedInUser);
  }, [artwork]);

  // Populate editing data when entering edit mode or when artwork changes
  useEffect(() => {
    if (isEditing && artwork && !isNew) {
      setEditingArtworkData({
        title: artwork.title || "",
        story: artwork.story || "",
        image: artwork.image || "",
        date: artwork.date || getFormattedDateOnly(new Date()),
        tipsAndTricks: artwork.tipsAndTricks || "",
        isPrivate: artwork.isPrivate || false,
        createdByArtistId: artwork.createdByArtistId || -1,
        postedByUserId: artwork.postedByUserId || -1,
        cityId: artwork.cityId || null,
        galleryId: artwork.galleryId || null,
      });
    }
    if (isEditing && isNew) {
      setEditingArtworkData(getInitialArtworkData(loggedInUser?.id ?? -1));
    }
  }, [isEditing, artwork, isNew, loggedInUser?.id]);

  useEffect(() => {
    if (isNew) {
      setIsEditing(isNew);
      setImgSrc("");
    }
  }, [isNew]);

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
    if (artwork && !isNew) {
      setEditingArtworkData({
        title: artwork.title || "",
        story: artwork.story || "",
        image: artwork.image || "",
        date: artwork.date || getFormattedDateOnly(new Date()),
        tipsAndTricks: artwork.tipsAndTricks || "",
        isPrivate: artwork.isPrivate || false,
        createdByArtistId: artwork.createdByArtistId || -1,
        postedByUserId: artwork.postedByUserId || -1,
        cityId: artwork.cityId || null,
        galleryId: artwork.galleryId || null,
      });
    } else if (isNew) {
      navigate(-1);
    }
  };

  const handleSave = async () => {
    if (!editingArtworkData.title.trim()) {
      alert("Title is required.");
      return;
    }
    if (editingArtworkData.title.length > 100) {
      alert("Title must be under 100 characters.");
      return;
    }

    if (isNew) {
      await addNewArtwork({
        ...editingArtworkData,
        image: editingArtworkData.image || "new",
        createdByArtistId: loggedInUser?.id ?? -1,
        postedByUserId: loggedInUser?.id ?? -1,
      });
      navigate("/profile");
    } else if (artwork) {
      await updateArtwork(artwork.id, editingArtworkData);
      setRefetchArtwork(!refetchArtwork);
      setIsEditing(false);
    }
  };

  const handleLike = async () => {
    if (artwork) {
      const liked = await likeArtwork(artwork.id);
      setIsLiked(liked);
    }
  };

  const handleDislike = async () => {
    if (artwork) {
      await dislikeArtwork(artwork.id);
      setIsLiked(false);
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

            {!isNew &&
              (isLiked ? (
                <IoMdHeart
                  className="ap-info-header-icon"
                  id="ap-liked-artwork-icon"
                  title="Dislike"
                  onClick={handleDislike}
                />
              ) : (
                <IoMdHeartEmpty
                  className="ap-info-header-icon"
                  title="Like"
                  onClick={handleLike}
                />
              ))}

            {!isNew && artwork?.postedByUserId === loggedInUser?.id && (
              <div className="ap-info-header-right-group">
                <MdEdit
                  className="ap-info-header-icon"
                  title="Edit"
                  onClick={() => setIsEditing((prev) => !prev)}
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
              {artwork?.date?.toString() ||
                (isEditing ? editingArtworkData.date.toString() : "-")}
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
                  : isEditing && loggedInUser?.userName
                  ? "@" + loggedInUser.userName
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
                  : isEditing && loggedInUser?.userName
                  ? "@" + loggedInUser.userName
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
              onClick={handleSave}
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
