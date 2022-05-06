using System;

namespace EAR.Entity
{
    public class EntityFactory
    {
        public static BaseEntity InstantNewEntity(EntityData entityData)
        {
            if (entityData is ButtonData buttonData)
            {
                return ButtonEntity.InstantNewEntity(buttonData);
            } else if (entityData is ModelData modelData)
            {
                return ModelEntity.InstantNewEntity(modelData);
            } else if (entityData is ImageData imageData)
            {
                return ImageEntity.InstantNewEntity(imageData);
            } else if (entityData is SoundData soundData)
            {
                return SoundEntity.InstantNewEntity(soundData);
            } else if (entityData is NoteData noteData)
            {
                return NoteEntity.InstantNewEntity(noteData);
            } else if (entityData is VideoData videoData)
            {
                return VideoEntity.InstantNewEntity(videoData);
            } else
            {
                throw new ArgumentException(entityData.ToString());
            }
        }
    }
}

