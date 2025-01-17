import React, { createContext, PropsWithChildren, useState } from "react";
import { userInfo } from "../types/userInfo";

interface AuthContextType {
    userInfo: userInfo;
    setUserInfo: React.Dispatch<React.SetStateAction<userInfo>>;
}

export const AuthContext = createContext<AuthContextType>({
    userInfo: {
        id: '',
        email: '',
        displayName: '',
        avatar: '',
        role: '',
    },
    setUserInfo: () => {},
});

const AuthContextProvider: React.FC = ({ children }: PropsWithChildren) => {
    const [userInfo, setUserInfo] = useState<userInfo>({
        id: '',
        email: '',
        displayName: '',
        avatar: '',
        role: ''
    });
    
    const contextValue = {
        userInfo,
        setUserInfo,
    }
    return (
        <AuthContext.Provider value={contextValue}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContextProvider;