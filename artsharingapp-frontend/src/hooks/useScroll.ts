import { useEffect } from "react";

export function useScroll({
  ref,
  storageKey,
  onReachBottom,
}: {
  ref: React.RefObject<HTMLDivElement | null>;
  storageKey: string;
  onReachBottom: () => void;
}) {
  useEffect(() => {
    const container = ref.current;
    if (!container) return;

    // Restore scroll position
    const savedScroll = sessionStorage.getItem(storageKey);
    if (savedScroll) {
      container.scrollTop = parseInt(savedScroll, 10);
    }

    let hasReachedBottom = false;

    const handleScroll = () => {
      const scrollTop = container.scrollTop;
      const clientHeight = container.clientHeight;
      const scrollHeight = container.scrollHeight;

      // Trigger function if scrolled near bottom
      const nearBottom = scrollTop + clientHeight >= scrollHeight - 5;

      if (nearBottom && !hasReachedBottom) {
        hasReachedBottom = true;
        onReachBottom();
      }

      // Save scroll position
      sessionStorage.setItem(storageKey, scrollTop.toString());
    };

    container.addEventListener("scroll", handleScroll);

    return () => {
      container.removeEventListener("scroll", handleScroll);
      sessionStorage.removeItem(storageKey); // when component unmounts reset scroll position
    };
  }, [onReachBottom]);
}
