import Dock from "../components/Dock";
import { useTranslation } from "react-i18next";
import "../styles/ArtworkPage.css";
import { useArtwork } from "../hooks/useArtwork";
import { PiDotsThreeOutlineVerticalFill } from "react-icons/pi";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import { MdEdit } from "react-icons/md";
import { IoMdHeartEmpty, IoMdHeart } from "react-icons/io";
import { FiUpload } from "react-icons/fi";
import { HiArrowPathRoundedSquare } from "react-icons/hi2";
import { CiLock, CiUnlock } from "react-icons/ci";
import { useEffect, useState } from "react";
import {
  addNewArtwork,
  ArtworkRequest,
  changeArtworkVisibility,
  dislikeArtwork,
  extractArtworkColor,
  likeArtwork,
  updateArtwork,
} from "../services/artwork";
import { useNavigate, useParams } from "react-router-dom";
import TextEditor from "../components/TextEditor";
import {
  ARTIST_FALLBACK_IMAGE,
  ARTWORK_FALLBACK_IMAGE,
  IMAGE_SERVICE_BASE_URL,
} from "../config/constants";
import AuctionSection from "../components/auctions-and-sales/AuctionSection";
import ThreeDotsMenu from "../components/ThreeDotsMenu";
import FixedSaleSection from "../components/auctions-and-sales/FixedSaleSection";
import { AuctionProvider } from "../context/AuctionContext";
import NotFound from "./NotFound";
import { toast } from "react-toastify";

type ArtworkPageProps = {
  isNew?: boolean;
};

const getFormattedDateOnly = (date: Date): string => {
  return date.toISOString().split("T")[0];
};

const getInitialArtworkData = (userId: number): ArtworkRequest => ({
  title: "",
  story: "<p></p>",
  date: getFormattedDateOnly(new Date()),
  tipsAndTricks: "<p></p>",
  isPrivate: false,
  createdByArtistId: userId,
  postedByUserId: userId,
  cityId: null,
  galleryId: null,
  color: null,
});

const ArtworkPage = ({ isNew = false }: ArtworkPageProps) => {
  const { t } = useTranslation();
  const { artworkId } = useParams();
  const navigate = useNavigate();
  const { loggedInUser } = useLoggedInUser();

  const [isEditing, setIsEditing] = useState<boolean>(isNew);
  const [isLiked, setIsLiked] = useState<boolean>(false);
  const [isPrivate, setIsPrivate] = useState<boolean>(false);

  const [refetchArtwork, setRefetchArtwork] = useState<boolean>(false);
  const [imgSrc, setImgSrc] = useState<string>("");
  const [artworkImageFile, setArtworkImageFile] = useState<File | null>(null);
  const [extractedColor, setExtractedColor] = useState<string | null>(null);
  const [is3DotsMenuOpen, setIs3DotsMenuOpen] = useState<boolean>(false);

  // Fetch artwork if not new
  const { artwork, loadingArtwork } = useArtwork(
    !isNew && artworkId ? parseInt(artworkId) : -1,
    refetchArtwork,
  );

  const [editingArtworkData, setEditingArtworkData] = useState<ArtworkRequest>(
    getInitialArtworkData(loggedInUser?.id ?? -1),
  );

  useEffect(() => {
    if (!artwork) return;

    setImgSrc(`${IMAGE_SERVICE_BASE_URL}${artwork.image}?t=${Date.now()}`);
    setExtractedColor(artwork.color);

    setIsLiked(!!artwork.isLikedByLoggedInUser);
    setIsPrivate(!!artwork.isPrivate);
  }, [artwork]);

  // Populate editing data when entering edit mode or when artwork changes
  useEffect(() => {
    if (isEditing && artwork && !isNew) {
      setEditingArtworkData({
        title: artwork.title || "",
        story: artwork.story || "",
        date: artwork.date || getFormattedDateOnly(new Date()),
        tipsAndTricks: artwork.tipsAndTricks || "",
        isPrivate: artwork.isPrivate || false,
        createdByArtistId: artwork.createdByArtistId || -1,
        postedByUserId: artwork.postedByUserId || -1,
        cityId: artwork.cityId || null,
        galleryId: artwork.galleryId || null,
        color: artwork.color || null,
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
      setExtractedColor(null);
    }
  }, [isNew]);

  useEffect(() => {
    if (!isNew) setIsEditing(false);
  }, [artworkId]);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>,
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
        date: artwork.date || getFormattedDateOnly(new Date()),
        tipsAndTricks: artwork.tipsAndTricks || "",
        isPrivate: artwork.isPrivate || false,
        createdByArtistId: artwork.createdByArtistId || -1,
        postedByUserId: artwork.postedByUserId || -1,
        cityId: artwork.cityId || null,
        galleryId: artwork.galleryId || null,
        color: artwork.color || null,
      });
      setExtractedColor(artwork.color);
      setImgSrc(`${IMAGE_SERVICE_BASE_URL}${artwork.image}?t=${Date.now()}`);
    } else if (isNew) {
      navigate(-1);
    }
  };

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    setArtworkImageFile(file);
    if (file) {
      const url = URL.createObjectURL(file);
      setImgSrc(url);

      const color = await extractArtworkColor(file);
      setExtractedColor(color);
      setEditingArtworkData((prev) => ({
        ...prev,
        color: color,
      }));
    }
  };

  const handleSave = async () => {
    if (!editingArtworkData.title.trim()) {
      toast.error(t("artwork.titleRequiredError"));
      return;
    }
    if (editingArtworkData.title.length > 100) {
      toast.error(t("artwork.titleTooLongError"));
      return;
    }

    if (isNew) {
      if (!artworkImageFile) {
        toast.error(t("artwork.imageRequiredError"));
        return;
      }
      const success = await addNewArtwork(
        {
          ...editingArtworkData,
          createdByArtistId: loggedInUser?.id ?? -1,
          postedByUserId: loggedInUser?.id ?? -1,
        },
        artworkImageFile,
      );
      if (success) {
        toast.success(t("artwork.addedSuccess"));
        navigate(`/${loggedInUser?.userName}`);
      }
    } else if (artwork) {
      const success = await updateArtwork(
        artwork.id,
        editingArtworkData,
        artworkImageFile,
      );
      if (success) {
        setRefetchArtwork((prev) => !prev);
        setIsEditing(false);
        toast.success(t("artwork.updatedSuccess"));
      }
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

  const handleChangeVisibility = async (visibility: "private" | "public") => {
    if (artwork) {
      await changeArtworkVisibility(artwork.id, {
        isPrivate: visibility === "private" ? true : false,
      });
      setIsPrivate((prev) => !prev);
    }
  };

  if (!loadingArtwork && !artwork && !isNew) {
    return <NotFound />;
  }

  return (
    <AuctionProvider artworkId={artwork?.id ?? -1}>
      <div className="artwork-page fixed-page">
        <div
          className="ap-image-container"
          style={
            {
              "--artwork-background-color": extractedColor,
            } as React.CSSProperties
          }
        >
          {imgSrc ? (
            <div className="ap-image-wrapper">
              <img
                src={imgSrc}
                alt={artwork?.title || t("common.artworkImageFallbackAlt")}
                onError={() => setImgSrc(ARTWORK_FALLBACK_IMAGE)}
                className="ap-image"
              />
              {isEditing && (
                <>
                  <div
                    className="ap-image ap-replace-image-overlay"
                    title={t("artwork.replaceImage")}
                  >
                    <HiArrowPathRoundedSquare />
                    <input
                      type="file"
                      accept="image/*"
                      onChange={handleFileChange}
                      className="ap-file-input-overlay"
                    />
                  </div>

                  <div className="ap-change-color-container">
                    <input
                      type="color"
                      name="color"
                      className="ap-color-picker"
                      title={t("artwork.changeColorBackground")}
                      value={extractedColor || artwork?.color || "#5c5c5c"}
                      onChange={(e) => {
                        handleInputChange(e);
                        setExtractedColor(e.target.value);
                      }}
                    />
                    <button
                      className="ap-color-btn"
                      title={t("artwork.removeColorBackground")}
                      onClick={() => {
                        setExtractedColor(null);
                        setEditingArtworkData((prev) => ({
                          ...prev,
                          color: null,
                        }));
                      }}
                    >
                      {t("artwork.clearColor")}
                    </button>
                    <button
                      className="ap-color-btn"
                      title={t("artwork.revertColorBackground")}
                      onClick={() => {
                        if (artwork) setExtractedColor(artwork.color);
                      }}
                    >
                      {t("artwork.revertColor")}
                    </button>
                  </div>
                </>
              )}
            </div>
          ) : isNew && !loadingArtwork ? (
            <div
              className="ap-upload-image ap-image"
              title={t("artwork.uploadImage")}
            >
              <FiUpload />
              <input
                type="file"
                accept="image/*"
                onChange={handleFileChange}
                className="ap-file-input-overlay"
              />
            </div>
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
                placeholder={t("artwork.titlePlaceholder")}
              />
            ) : (
              <h1 className="ap-title">{artwork?.title || ""}</h1>
            )}
            <div className="ap-info-header-right-group">
              {artwork?.isOnSale && (
                <div className="ap-on-sale">{t("common.onSale")}</div>
              )}

              {!isNew &&
                (isLiked ? (
                  <IoMdHeart
                    className="ap-info-header-icon"
                    id="ap-liked-artwork-icon"
                    title={t("artwork.dislike")}
                    onClick={handleDislike}
                  />
                ) : (
                  <IoMdHeartEmpty
                    className="ap-info-header-icon"
                    title={t("artwork.like")}
                    onClick={handleLike}
                  />
                ))}

              {!isNew && artwork?.postedByUserId === loggedInUser?.id && (
                <div className="ap-info-header-right-group">
                  {isPrivate ? (
                    <CiLock
                      className="ap-info-header-icon"
                      title={t("artwork.private")}
                      onClick={() => handleChangeVisibility("public")}
                    />
                  ) : (
                    <CiUnlock
                      className="ap-info-header-icon"
                      title={t("artwork.public")}
                      onClick={() => handleChangeVisibility("private")}
                    />
                  )}
                  <MdEdit
                    className="ap-info-header-icon"
                    title={t("common.edit")}
                    onClick={() => setIsEditing((prev) => !prev)}
                  />
                  <>
                    <PiDotsThreeOutlineVerticalFill
                      className="ap-info-header-icon"
                      id="ap-3-dots-menu"
                      title={t("artwork.options")}
                      onClick={(e) => {
                        e.stopPropagation();
                        setIs3DotsMenuOpen((prev) => !prev);
                      }}
                    />
                    {is3DotsMenuOpen && (
                      <ThreeDotsMenu
                        artworkId={artworkId ? parseInt(artworkId) : -1}
                        onClose={() => setIs3DotsMenuOpen(false)}
                        refetchArtwork={() =>
                          setRefetchArtwork((prev) => !prev)
                        }
                      />
                    )}
                  </>
                </div>
              )}
            </div>
          </div>

          <div className="ap-details">
            <div className="ap-date">
              <p className="ap-details-label">{t("artwork.dateLabel")}</p>
              <p className="ap-details-text">
                {artwork?.date?.toString() ||
                  (isEditing ? editingArtworkData.date.toString() : "-")}
              </p>
            </div>
            <div className="ap-user-profile">
              <p className="ap-details-label">{t("artwork.createdByLabel")}</p>
              {artwork?.createdByArtistUserName ||
              (isEditing && loggedInUser?.userName) ? (
                <div className="ap-user-info">
                  <img
                    src={`${IMAGE_SERVICE_BASE_URL}${artwork?.createdByArtistProfilePhoto ?? loggedInUser?.profilePhoto}`}
                    alt=""
                    className="ap-user-profile-picture"
                    onError={(e) => {
                      console.log(loggedInUser);
                      e.currentTarget.src = ARTIST_FALLBACK_IMAGE;
                    }}
                  />
                  <span className="ap-details-text">
                    {"@" +
                      (artwork?.createdByArtistUserName ||
                        loggedInUser?.userName)}
                  </span>
                </div>
              ) : (
                <span className="ap-details-text">-</span>
              )}
            </div>
            <div className="ap-user-profile">
              <p className="ap-details-label">{t("artwork.postedByLabel")}</p>
              {artwork?.postedByUserName ||
              (isEditing && loggedInUser?.userName) ? (
                <div className="ap-user-info">
                  <img
                    src={`${IMAGE_SERVICE_BASE_URL}${artwork?.postedByUserProfilePhoto ?? loggedInUser?.profilePhoto}`}
                    alt=""
                    className="ap-user-profile-picture"
                    onError={(e) => {
                      e.currentTarget.src = ARTIST_FALLBACK_IMAGE;
                    }}
                  />
                  <span className="ap-details-text">
                    {"@" +
                      (artwork?.postedByUserName || loggedInUser?.userName)}
                  </span>
                </div>
              ) : (
                <span className="ap-details-text">-</span>
              )}
            </div>
          </div>

          {artwork?.isOnSale && <FixedSaleSection artwork={artwork} />}

          <AuctionSection />

          {((artwork?.story && artwork.story !== "<p></p>") || isEditing) && (
            <div className="ap-story">
              <h3>{t("artwork.storyHeading")}</h3>
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
              <h3>{t("artwork.tipsAndTricksHeading")}</h3>
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
                {t("common.cancel")}
              </button>
              <button
                className="ap-save-edit-btn"
                onClick={handleSave}
                type="button"
              >
                {t("common.save")}
              </button>
            </div>
          )}
        </div>
        <Dock />
      </div>
    </AuctionProvider>
  );
};

export default ArtworkPage;
